using System;
using System.Collections.Generic;
using Rentitas.Kernel;

namespace Rentitas
{
    public interface IKernel
    {
        BaseScenario Scenario { get; }
        IPool[] PoolInterfaces { get; }
    }
}