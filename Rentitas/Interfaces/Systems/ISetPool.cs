namespace Rentitas
{
    public interface ISetPools : ISystem
    {
        void SetPools(Pools pools);
    }

    public interface ISetPool<T> : ISystem where T : class, IComponent
    {
        void SetPool(Pool<T> typedPool);
    }
}