using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rentitas;

public class SampleApp : MonoBehaviour, IKernel {
    public Scenario AppScenario { get; private set; }
    public List<IPool> SetupPools()
    {
        return null;
    }
}
