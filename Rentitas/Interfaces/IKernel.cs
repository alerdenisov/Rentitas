namespace Rentitas
{
    public interface IKernel
    {
        IPool[] PoolInterfaces { get; }
        BaseScenario SetupScenario(Pools pools);
    }
}