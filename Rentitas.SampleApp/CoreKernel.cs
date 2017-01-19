using System;
using Rentitas.Kernel;
using Rentitas.Unity;

namespace Rentitas.SampleApp
{
    public class CoreKernel : IKernel
    {
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

        }

        public BaseScenario SetupScenario(Pools pools)
        {
            return new Scenario("Core Scenario");
        }
    }
}