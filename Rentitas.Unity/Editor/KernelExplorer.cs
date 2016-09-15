using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rentitas.Unity
{
    public class KernelExplorer : EditorWindow
    {
        public RentitasExecutor Executor { get; set; }
        public KernelBehaviour Kernel { get; set; }
        public Type SelectedPoolType { get; set; }
        public Rect PoolRect { get; set; }

        public Type SelectedComponentType { get; set; }
        public Rect ComponentRect { get; set; }
        public RentitasExecutor.PoolInstallConfig SelectedConfig { get; set; }

        private IKernelExplorerWidget _widget;

        private void SetupWidgets()
        {
            _widget = new  MainWidget();
        }

        #region execute
        [MenuItem("Window/Rentitas/Kernel Explorer")]
        public static void Open()
        {
            var instance = EditorWindow.GetWindow<KernelExplorer>();
            instance.Show();
        }

        void Awake()
        {
            SetupWidgets();
        }

        void Start()
        {
            SetupWidgets();
        }

        void OnGUI()
        {
            if (_widget == null)
            {
                SetupWidgets();
                return;
            }

            _widget.DrawWidget(this);
            Repaint();
        }

        #endregion
    }
}