using UnityEditor;
using UnityEngine;

namespace Rentitas.Unity
{
    public class DrawSelectFirstWidget : IKernelExplorerWidget
    {
        public void DrawWidget(KernelExplorer explorer)
        {
            GUILayout.Label("Select Executor or KernelBehaviour first!", EditorStyles.boldLabel);
        }
    }
}