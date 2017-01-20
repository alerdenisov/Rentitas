using UnityEditor;
using UnityEngine;

namespace Rentitas.Unity.VisualDebugging
{
    [CustomEditor(typeof(Inspect))]
    public class InspectMonitor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (UnityEngine.Application.isPlaying)
                return;

            base.OnInspectorGUI();
        }
    }
}