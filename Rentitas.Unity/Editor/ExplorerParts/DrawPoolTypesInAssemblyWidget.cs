using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rentitas.Unity
{
    public class DrawPoolTypesInAssemblyWidget : IKernelExplorerWidget
    {
        private KernelExplorer _e;
        private IEnumerable<Type> _types;
        private Dictionary<Type, string> _typeNames; 
        private Assembly _assembly;

        private GUIStyle _unselectedStyle;
        private GUIStyle _selectedStyle;

        public DrawPoolTypesInAssemblyWidget()
        {
            var style = _unselectedStyle = new GUIStyle(GUI.skin.GetStyle("flow node 1"));
            style.alignment = TextAnchor.MiddleCenter;
            style.padding = new RectOffset(10, 10, 30, -12);
            style.margin = new RectOffset(10, 10, 10, 10);
            style.normal.textColor = Color.black;


            var style2 = _selectedStyle = new GUIStyle(GUI.skin.GetStyle("flow node 1 on"));
            style2.alignment = TextAnchor.MiddleCenter;
            style2.padding = new RectOffset(10, 10, 30, -12);
            style2.margin = new RectOffset(10, 10, 10, 10);
            style2.normal.textColor = Color.white;
        }

        protected KernelExplorer E
        {
            get { return _e; }
            set
            {
                if (_e != value)
                {
                    _e = value;
                    SetupContext();
                }
            }
        }

        private void SetupContext()
        {
            if (E == null) return;
            _assembly = Assembly.GetAssembly(E.Executor.GetType());
            _types = _assembly.GetTypes().Where(t => typeof (IComponent).IsAssignableFrom(t) && t.IsInterface && t != typeof(IComponent));
            _typeNames = _types.ToDictionary(t => t, t => t.ToString().Split(new[] {'.'}).Last());

        }

        public Rect StartRect { get; set; }


        public void DrawWidget(KernelExplorer explorer)
        {
            E = explorer;

            if (E.Executor.Configs.Length != _types.Count())
                E.Executor.IsNotConfigurated = false;

            if (_types == null)
            {
                GUILayout.Label("Create interfaces implemented IComponent to define Pool Types");
            }
            else
            {
                var rect = new Rect(StartRect);

                rect.x += 30;
                rect.width = 300;
                rect.height = 40;

                foreach (var type in _types)
                {
                    var selected = explorer.SelectedPoolType == type;
                    var style = selected
                        ? _selectedStyle
                        : _unselectedStyle;
                    if (GUI.Button(rect, _typeNames[type], style))
                        explorer.SelectedPoolType = type;


                    var pinRect = new Rect(rect);

                    pinRect.x -= 18;
                    pinRect.y += 14;
                    pinRect.width = 13;
                    pinRect.height = 13;

                    var config = explorer.Executor.GetConfig(type);

                    GUI.color = Color.green;
                    if (GUI.Button(pinRect, "", !config.IsActive ? "WinBtnCloseMac" : "WinBtnMaxMac"))
                        config.IsActive = !config.IsActive;
                    GUI.color = Color.white;

                    if (selected)
                    {
                        explorer.PoolRect = new Rect(rect);
                        explorer.SelectedConfig = config;
                    }

                    rect.y += 40;
                }
            }
        }
    }
}