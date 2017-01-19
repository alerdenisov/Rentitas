using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rentitas.Unity.VisualDebugging
{
    public interface IAppProvider
    {
        Rentitas.Application Application { get; }
    }
    public class Inspect : MonoBehaviour
    {
        public Component AppProvider;
        public IAppProvider Boot;
        public GUISkin Skin;

        private GUIStyle _fill;
        private Texture2D _fillTexture;

        protected Texture2D FillTexture
        {
            get
            {
                if (!_fillTexture)
                {
                    _fillTexture = new Texture2D(1, 1);
                    _fillTexture.SetPixel(0, 0, Color.white);
                    _fillTexture.Apply();
                }
                return _fillTexture;
            }
        }

        protected GUIStyle Fill
        {
            get
            {
                if (_fill == null)
                {
                    _fill = new GUIStyle();
                    _fill.normal.background = FillTexture;
                    _fill.normal.textColor = Color.black;
                }
                return _fill;
            }
        }

        protected bool ShowInspector;

        void Awake()
        {
            if(!(AppProvider is IAppProvider))
                throw new ArgumentException("App Provider must implement IAppProvider interface!");

            Boot = (IAppProvider) AppProvider;
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.F2))
                ShowInspector = !ShowInspector;
        }

        public void OnGUI()
        {
            //GUI.skin = Skin;
            if (ShowInspector)
            {
                var tmp = GUI.color;
                GUI.color = Color.black;
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", Fill);
                GUI.color = tmp;

                GUI.Window(0, new Rect(0, 0, Screen.width, Screen.height), Window, "Inspector");
            }
        }

        private Vector2 _poolScroll;
        private Vector2 _entyScroll;
        private Vector2 _prevScroll;
        private IPool _selectedPool;
        private object _selectedEntity;

        private Dictionary<Type, FieldInfo> _entityFields = new Dictionary<Type, FieldInfo>();
        private Dictionary<Type, MethodInfo> _entityComponents = new Dictionary<Type, MethodInfo>();
        private Dictionary<Type, FieldInfo[]> _componentFields = new Dictionary<Type, FieldInfo[]>();
        private Dictionary<Type, Action<object>> _componentDrawers = new Dictionary<Type, Action<object>>();
        private Dictionary<Type, PropertyInfo[]> _componentProperties = new Dictionary<Type, PropertyInfo[]>();
        private Dictionary<Type, string> _typeNames = new Dictionary<Type, string>();

        private string _searchString = "";
        private object _selectedComponent;
        private bool _showOwners;

        private void Window(int id)
        {
            _searchString = GUILayout.TextField(_searchString);

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.Width(Screen.width / 4f));
                {
                    DrawPoolSelection();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.Width(Screen.width / 4f));
                {
                    DrawEntitySelection();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.Width(Screen.width / 2f));
                {
                    DrawComponentsPreview();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawComponentsPreview()
        {
            if (GUILayout.Button("Show " + (_showOwners ? "Components" : "Owners")))
                _showOwners = !_showOwners;

            _prevScroll = GUILayout.BeginScrollView(_prevScroll, false, true);
            {
                if (_selectedEntity != null)
                {
                    if (_showOwners)
                    {
                        var type = _selectedEntity.GetType();
                        var info = type.GetField("owners", BindingFlags.Public | BindingFlags.Instance);
                        var owners = (HashSet<object>) info.GetValue(_selectedEntity);

                        foreach (var owner in owners)
                        {
                            GUILayout.Label(owner.ToString());
                        }

                    }
                    else
                    {
                        var type = _selectedEntity.GetType();
                        if (!_entityComponents.ContainsKey(type))
                        {
                            var info = type.GetMethod("GetComponents", BindingFlags.Public | BindingFlags.Instance);
                            _entityComponents.Add(type, info);
                        }

                        var field = _entityComponents[type];
                        if (field != null)
                        {
                            var dict = (IEnumerable) field.Invoke(_selectedEntity, new object[0]);
                            foreach (var comp in dict)
                            {
                                if (GUILayout.Button(TypeName(comp?.GetType())))
                                    _selectedComponent = _selectedComponent == comp ? null : comp;

                                if (comp?.GetType() == _selectedComponent?.GetType())
                                    _selectedComponent = comp;

                                if (_selectedComponent == comp)
                                    DrawComponentFields(_selectedComponent);
                            }
                        }
                    }
                }
            }
            GUILayout.EndScrollView();
        }

        private void DrawComponentFields(object selectedComponent)
        {
            var type = selectedComponent.GetType();
            if (!_componentDrawers.ContainsKey(type))
            {
                var customDrawer = GetExtensionMethods(type.Assembly, type).FirstOrDefault(m => m.Name == "InspectorGUI");//type.GetMethod("InspectorGUI", BindingFlags.Static | BindingFlags.Public));
                if (customDrawer != null)
                {
                    _componentDrawers.Add(type, o => customDrawer.Invoke(null, new[] { o }));
                }
                else
                {
                    _componentDrawers.Add(type, DefaultComponentInspectorGUI);
                }
            }

            _componentDrawers[type](selectedComponent);
        }

        static IEnumerable<MethodInfo> GetExtensionMethods(Assembly assembly, Type extendedType)
        {
            var query = from type in assembly.GetTypes()
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static
                        | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false)
                        where method.GetParameters()[0].ParameterType == extendedType
                        select method;
            return query;
        }

        private void DefaultComponentInspectorGUI(object component)
        {
            var type = component.GetType();
            if (!_componentFields.ContainsKey(type))
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
                _componentFields.Add(type, fields);
            }

            if (!_componentProperties.ContainsKey(type))
            {
                var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                _componentProperties.Add(type, props);
            }

            foreach (var fieldInfo in _componentFields[type])
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(fieldInfo.Name, GUILayout.Width(150));
                    GUILayout.Label(TypeName(fieldInfo.FieldType), GUILayout.Width(100));
                    GUILayout.Label(fieldInfo.GetValue(component)?.ToString());
                }
                GUILayout.EndHorizontal();
            }

            foreach (var propInfo in _componentProperties[type])
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(propInfo.Name, GUILayout.Width(150));
                    GUILayout.Label(TypeName(propInfo.PropertyType), GUILayout.Width(100));
                    GUILayout.Label(propInfo.GetValue(component, null)?.ToString());
                }
                GUILayout.EndHorizontal();
            }
        }

        private string TypeName(Type type)
        {
            if (type == null) return "Null";

            if (!_typeNames.ContainsKey(type))
            {
                _typeNames.Add(type, type.ToString().Split('.').Last());
            }

            return _typeNames[type];
        }

        private void DrawEntitySelection()
        {
            _entyScroll = GUILayout.BeginScrollView(_entyScroll, false, true);
            {
                if (_selectedPool != null)
                {
                    var type = _selectedPool.GetType();
                    if (type.BaseType != null && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                        type = type.BaseType;

                    if (!_entityFields.ContainsKey(type))
                    {
                        var fieldInfo = type.GetField("_entities",
                            BindingFlags.NonPublic | BindingFlags.Instance);
                        _entityFields.Add(type, fieldInfo);
                    }

                    FieldInfo field = _entityFields[type];
                    var entities = field?.GetValue(_selectedPool) as IEnumerable;
                    if (entities != null)
                    {
                        var index = 0;
                        foreach (var entity in entities)
                        {
                            if (!string.IsNullOrEmpty(_searchString) && !entity.ToString().Contains(_searchString))
                                continue;

                            if (GUILayout.Button($"{entity.ToString()}"))
                            {
                                _selectedEntity = entity;
                            }

                            if (index++ > 50)
                                break;
                        }
                    }
                }
            }
            GUILayout.EndScrollView();
        }

        private void DrawPoolSelection()
        {
            _poolScroll = GUILayout.BeginScrollView(_poolScroll, false, true);
            {
                foreach (var pool in Boot.Application.Pools.GetPools())
                {
                    if (GUILayout.Button($"{pool.PoolName} {pool.PoolType}"))
                    {
                        _selectedPool = pool;
                    }
                }
            }
            GUILayout.EndScrollView();
        }
    }
}