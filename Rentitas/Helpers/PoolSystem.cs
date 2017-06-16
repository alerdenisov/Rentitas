namespace Rentitas
{
    public abstract class PoolSystem<T> : ISetPool<T> where T : class, IComponent
    {
        protected Pool<T> Pool { get; private set; }

        public void SetPool(Pool<T> typedPool)
        {
            this.Pool = typedPool;
            AfterSetPool();
        }

        protected virtual void AfterSetPool()
        {
        }
    }
}