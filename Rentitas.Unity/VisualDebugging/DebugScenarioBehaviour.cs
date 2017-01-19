using UnityEngine;

namespace Rentitas.Unity.VisualDebugging
{
    public class DebugScenarioBehaviour : MonoBehaviour
    {
        public DebugScenario Scenario { get; private set; }

        public void Init(DebugScenario debugScenario)
        {
            Scenario = debugScenario;
        }
    }
}