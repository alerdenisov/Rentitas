using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rentitas.Unity
{
    public class DrawComponentTypesInSelectedPool : IKernelExplorerWidget
    {
        private Type[] _poolComponents;
        private Dictionary<Type, string> _componentNames;
        private Type _poolType;

        private GUIStyle _componentStyleOff;
        private GUIStyle _componentStyleOn;
        private GUIStyle _componentPath;
        private Texture2D _pathTexture;

        protected GUIStyle ComponentPath
        {
            get
            {
                if (_componentPath == null) SetupResource();
                return _componentPath;
            }
        }
        protected GUIStyle ComponentStyleOff
        {
            get
            {
                if (_componentStyleOff == null) SetupResource();
                return _componentStyleOff;
            }
        }
        protected GUIStyle ComponentStyleOn
        {
            get
            {
                if (_componentStyleOn == null) SetupResource();
                return _componentStyleOn;
            }
        }

        protected Texture2D PathTexture
        {
            get
            {
                if (!_pathTexture) SetupResource();

                return _pathTexture;
            }
        }

        private void SetupResource()
        {
            _pathTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false, true);
            _pathTexture.SetPixels32(new Color32[] { Color.white, Color.white, Color.white, Color.white });
            _pathTexture.Apply(false, true);

            _componentPath = new GUIStyle();
            _componentPath.normal.background = _pathTexture;
            _componentPath.border = new RectOffset(1,1,1,1);

            _componentStyleOn = new GUIStyle(GUI.skin.GetStyle("flow var 4"));
            _componentStyleOn.padding.top += 1;
            _componentStyleOff = new GUIStyle(GUI.skin.GetStyle("flow var 5"));
            _componentStyleOff.padding.top += 1;
            _componentStyleOff.active = _componentStyleOff.hover = _componentStyleOn.normal;
        }

        public Type PoolType
        {
            get { return _poolType; }
            set
            {
                if (_poolType != value)
                {
                    _poolType = value;
                    Setup();
                }
            }
        }

        public Rect PoolRect { get; set; }

        private void Setup()
        {
            if (_poolType == null) return;
            var assembly = Assembly.GetAssembly(_poolType);
            _poolComponents = assembly.GetTypes().Where(t => _poolType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && t.GetConstructor(new Type[] {}) != null).ToArray();
            _componentNames = _poolComponents.ToDictionary(t => t, t => t.ToString().Split(new[] {'.'}).Last());
        }

        public void DrawWidget(KernelExplorer explorer)
        {
            var height = 20;
            var start = new Rect(PoolRect.x + PoolRect.width + 20, PoolRect.y, 300, 30);
            if (_poolComponents.Length > 0)
            {
                GUI.color = new Color(.6f,.6f,.6f,1f);
                GUI.Box(new Rect(start.x - 20, start.y + 6, 10, 1), "", ComponentPath);
                GUI.Box(new Rect(start.x - 10, start.y + 6, 1, height * (_poolComponents.Length - 1)), "", ComponentPath);

                for (int i = 0; i < _poolComponents.Length; i++)
                {
                    GUI.color = new Color(.6f, .6f, .6f, 1f);
                    GUI.Box(new Rect(start.x - 10, start.y + 6 + height * i, 10, 1), "", ComponentPath);
                    GUI.color = Color.white;

                    var component = _poolComponents[i];
                    var name = _componentNames[component];
                    var selected = explorer.SelectedComponentType == component;
                    var rect = new Rect(start.x, start.y + height*i, 200, 15);
                    var style = selected ? ComponentStyleOn : ComponentStyleOff;

                    var tmp = GUI.depth;
                    if (GUI.Button(rect, name, style))
                        explorer.SelectedComponentType = _poolComponents[i];

                    var pinRect = new Rect(rect);

                    pinRect.x -= 16;
                    pinRect.y += 1;
                    pinRect.width = 13; 
                    pinRect.height = 13;

                    var config = explorer.SelectedConfig.ComponentTypes[i];//.First(c => c.ComponentType.Type == component);

                    GUI.depth = tmp + 1;
                    GUI.color = Color.green;
                    if (GUI.Button(pinRect, "", !config.IsActive ? "WinBtnCloseMac" : "WinBtnMaxMac"))
                    {
                        Debug.Log("Toggle");
                        config.IsActive = !config.IsActive;
                    }
                    GUI.color = Color.white;

                    if (selected)
                        explorer.ComponentRect = rect;

                    GUI.depth = tmp;
                }
            }
        }
    }
}