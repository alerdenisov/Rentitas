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
            MainScenario.Add(kernel.Scenario);
            foreach (var poolInterface in kernel.PoolInterfaces)
            {
                Pools.Register(poolInterface);
            }

        }

        public virtual BaseScenario MainScenario { get; private set; }
    }
}