using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using Rentitas.Kernel;

namespace Rentitas
{
    public interface IKernel
    {
        //BaseScenario Scenario { get; }
        IPool[] PoolInterfaces { get; }
        BaseScenario SetupScenario(Pools pools);
    }
}