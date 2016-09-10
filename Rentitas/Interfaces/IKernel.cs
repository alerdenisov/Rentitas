using System.Collections.Generic;

namespace Rentitas
{
    public interface IKernel
    {
        Scenario AppScenario { get; }
        List<IPool> SetupPools();
    }
}