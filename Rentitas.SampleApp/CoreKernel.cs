using Rentitas.Kernel;
using Rentitas.Unity;

namespace Rentitas.SampleApp
{
    public class CoreKernel : IKernel
    {
        public BaseScenario Scenario { get; private set; }
        public IPool[] PoolInterfaces { get; private set; }

        public CoreKernel()
        {
            PoolInterfaces = new[]
            {
                new Pool<ICorePool>(
                    new TimerComponent(),
                    new GameStateComponent(),
                    new PauseComponent(),
                    new SettingsComponent())
            };

            Scenario = new Scenario("Core Scenario");
        }
    }
}