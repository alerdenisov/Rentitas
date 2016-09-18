namespace Rentitas
{
    public class Application : IApplication
    {
        public Pools Pools { get; }

        public Application()
        {
            Pools = new Pools();
            MainScenario = new BaseScenario("Main");
        }

        public virtual void RegisterKernel(IKernel kernel)
        {
            kernel.Scenario.SetApplication(this);
            foreach (var poolInterface in kernel.PoolInterfaces)
            {
                Pools.Register(poolInterface);
            }
            kernel.Scenario.SetPools(Pools);

            MainScenario.Add(kernel.Scenario);
            MainScenario.Initialize();
        }

        public virtual void UnregisterKernel(IKernel kernel)
        {
            MainScenario.Remove(kernel.Scenario);
        }

        public void Execute()
        {
            MainScenario.Execute();
            MainScenario.Cleanup();
        }

        public virtual BaseScenario MainScenario { get; private set; }
    }
}