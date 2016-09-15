using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rentitas.Unity
{
    public class DrawComponentFieldsWidget : IKernelExplorerWidget
    {
        private Type _componentType;
        private GUIStyle _componentPath;
        private Texture2D _pathTexture;
        private FieldInfo[] _componentFields;

        protected GUIStyle ComponentPath
        {
            get
            {
                if (_componentPath == null) SetupResource();
                return _componentPath;
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
            _componentPath.border = new RectOffset(1, 1, 1, 1);
        }
        
        public Rect ComponentRect { get; set; }

        public Type ComponentType
        {
            get { return _componentType; }
            set
            {
                if (_componentType != value)
                {
                    _componentType = value;
                    Setup();
                }
            }
        }

        private void Setup()
        {
            if (ComponentType == null) return;
            _componentFields = ComponentType.GetFields();
        }

        public void DrawWidget(KernelExplorer explorer)
        {
            var rect = new Rect(ComponentRect);
            rect.x += rect.width + 20;
            rect.width = 300;
            rect.height = 20;

            GUI.color = new Color(.6f, .6f, .6f, 1f);
            GUI.Box(new Rect(rect.x - 20, rect.y + 7, 10, 1), "", ComponentPath);
            GUI.Box(new Rect(rect.x - 10, rect.y + 7, 1, 20 * _componentFields.Length), "", ComponentPath);

            rect.width = 100;
            GUI.Box(rect, "Type");
            rect.width = 200;
            rect.x += 100;
            GUI.Box(rect, "Name");
            rect.x -= 100;
            rect.y += 20;

            for (int i = 0; i < _componentFields.Length; i++)
            {
                var componentField = _componentFields[i];
                GUI.color = new Color(.6f, .6f, .6f, 1f);
                GUI.Box(new Rect(rect.x - 10, rect.y + 7, 10, 1), "", ComponentPath);
                GUI.color = Color.white;

                rect.width = 100;
                GUI.Box(rect, componentField.FieldType.ToString());
                rect.width = 200;
                rect.x += 100;
                GUI.Box(rect, componentField.Name);
                rect.x -= 100;
                rect.y += 20;
            }
        }
    }
}