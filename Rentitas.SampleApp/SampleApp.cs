using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rentitas;
using Rentitas.Unity;

public class SampleApp : MonoBehaviour {
    public BaseScenario AppScenario { get; private set; }
    public List<IPool> SetupPools()
    {
        return null;    
    }

    void Awake()
    {
        AppScenario = new Scenario("Test");
    }
}
