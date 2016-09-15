using UnityEngine;

namespace Rentitas.Unity
{
    public class MainWidget : IKernelExplorerWidget
    {
        private IKernelExplorerWidget _targetSelect;

        public MainWidget()
        {
            _targetSelect = new TargetSelectionWidget();
        }

        public void DrawWidget(KernelExplorer explorer)
        {
            GUILayout.BeginHorizontal(GUI.skin.button);
            {
                GUILayout.Label("Kernel Explorer");
                if(GUILayout.Button("Test", GUILayout.Width(35)))
                    Debug.Log("Test");
            }
            GUILayout.EndHorizontal();

            _targetSelect.DrawWidget(explorer);
        }
    }
}