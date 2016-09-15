using Rentitas.Kernel;
using Rentitas.Unity;

namespace Rentitas.SampleApp
{
    public class CoreKernel// : IKernel
    {
        public BaseScenario Scenario => _kernelScenario;
        public PoolRegistrationData[] PoolInterfaces => _poolRegistration;


        private Scenario _kernelScenario;
        private PoolRegistrationData[] _poolRegistration;

        public CoreKernel()
        {
            _poolRegistration = new[]
            {
                new PoolRegistrationData(typeof(ICorePool)), 
                new PoolRegistrationData(typeof(ICorePool), "Temp"), 
            };
        }
    }
}