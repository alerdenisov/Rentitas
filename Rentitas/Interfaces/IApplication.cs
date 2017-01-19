using System;

namespace Rentitas
{
    /// <summary>
    /// 
    /// IApplication
    /// - MainScenario
    /// - - IKernel.Scenerio #0
    /// - - IKernel.Scenerio #1
    /// - - IKernel.Scenerio #2
    /// - - IKernel.Scenerio #3
    /// - - IKernel.Scenerio #4
    /// 
    /// 
    /// 
    /// 
    /// </summary>


    public interface IApplication : IDisposable
    {
        Pools Pools { get; }
        BaseScenario MainScenario { get; }
        void RegisterKernel(IKernel kernel);
        void UnregisterKernel(IKernel kernelA);
        void Execute();
    }
}