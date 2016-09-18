#if UNITY_EDITOR
using Rentitas.Unity.VisualDebugging;
#endif

namespace Rentitas.Unity
{
#if UNITY_EDITOR
    public class Scenario : BaseScenario
#else
    public class Scenario : BaseScenario
#endif
    {
        public Scenario(string name = "Systems") : base(name)
        {
        }
    }
}