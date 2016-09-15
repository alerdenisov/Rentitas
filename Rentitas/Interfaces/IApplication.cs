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


    public interface IApplication
    {
        Pools Pools { get; }
        void RegisterKernel(IKernel kernel);
        BaseScenario MainScenario { get; }
    }
}