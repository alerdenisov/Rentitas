using Rentitas.Unity.VisualDebugging;
using UnityEngine;

namespace Rentitas.Unity
{
    public class Step : MonoBehaviour
    {
        public bool Stepping;
    }
    public class StepScenario : BaseScenario
    {
        private Step _stepper;
        private int _currentIndex;
        public override void Execute()
        {
            if (!_stepper.gameObject.activeSelf)
                return;

            var system = ExecuteSystems[_currentIndex];
            if (system is IReactiveInternalSystem)
            {
                Debug.Log("Execute system " + ((system as IReactiveInternalSystem).InternalSubsystem).GetType());
            }
            else
            {
                Debug.Log("Execute system " + system.GetType());
            }
            system.Execute();

            _currentIndex++;
            if (_currentIndex >= ExecuteSystems.Count)
            {
                _currentIndex = 0;
                foreach (var cleanupSystem in CleanupSystems)
                {
                    cleanupSystem.Cleanup();
                }
                _stepper.gameObject.SetActive(false);
            }

            if(_stepper.Stepping)
                _stepper.gameObject.SetActive(false);
        }

        public override void Cleanup()
        {
//            base.Cleanup();
        }

        public StepScenario(string name = "Systems") : base(name)
        {
            var go = new GameObject("STEPPER " + name);
            _stepper = go.AddComponent<Step>();
            _stepper.gameObject.SetActive(false);
        }
    }
#if UNITY_EDITOR
    public class Scenario : DebugScenario
    {
#else
    public class Scenario : BaseScenario
    {
#endif
        public Scenario(string name = "Systems") : base(name)
        {
        }
    }
}
