using System.Collections.Generic;

namespace Rentitas
{
    public abstract class Application : IApplication
    {
        public Pools Pools { get; }

        private Dictionary<IKernel, BaseScenario> _kernelScenarios;

        protected Application()
        {
            _kernelScenarios = new Dictionary<IKernel, BaseScenario>();
            Pools = new Pools();
            MainScenario = new BaseScenario("Main");
        }

        public void RegisterKernel(IKernel kernel)
        {
            if (kernel.PoolInterfaces != null)
            {
                foreach (var poolInterface in kernel.PoolInterfaces)
                {
                    Pools.Register(poolInterface);
                }
            }

            var scenario = kernel.SetupScenario(Pools);
            _kernelScenarios.Add(kernel, scenario);

            scenario.SetPools(Pools);

            scenario.SetApplication(this);
            MainScenario.Add(scenario);
            scenario.Initialize();
        }

        public void UnregisterKernel(IKernel kernel)
        {
            if (!_kernelScenarios.ContainsKey(kernel))
            {
                return;
            }
            MainScenario.Remove(_kernelScenarios[kernel]);
            _kernelScenarios.Remove(kernel);
        }

        public virtual void Execute()
        {
            if(!IsStarted)
                Start();

            MainScenario.Execute();
            MainScenario.Cleanup();
        }

        public virtual void Start()
        {
            IsStarted = true;
        }

        public bool IsStarted { get; private set; }

        public BaseScenario MainScenario { get; }

        public void Dispose()
        {
            MainScenario.Deinitialize();
        }
    }
}