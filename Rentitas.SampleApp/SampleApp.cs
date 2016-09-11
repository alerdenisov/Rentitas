using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rentitas;

public interface ICoreComponent : IComponent { }

public class SampleApp : MonoBehaviour, IKernel {
    public Scenario AppScenario { get; private set; }
    public List<IPool> SetupPools()
    {
        return null;
    }

    void Test()
    {
        var e = new Entity<ICoreComponent>(null, null);
    }
}
