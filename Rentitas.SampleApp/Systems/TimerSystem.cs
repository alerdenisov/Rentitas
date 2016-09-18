namespace Rentitas.SampleApp.Systems
{
    public class TimerSystem : IExecuteSystem, ISetPools
    {
        private Pool<ICorePool> _corePool;

        public void Execute()
        {
        }

        public void SetPools(Pools pool)
        {
            _corePool = pool.Get<ICorePool>();
        }
    }
}