using System;

namespace Rentitas
{
    [Obsolete("Use ISetPool instead of ISetPools. \n" +
              "TIP: Inject dependencies as a kernel constructor params and save in pool inside ISingleton component", false)]
    public interface ISetPools : ISystem
    {
        void SetPools(Pools pools);
    }

    /// <summary>
    /// Mark system as a pool instance required.
    /// Will receive pool instance in creation cycle
    /// </summary>
    /// <typeparam name="T">Interface based pool type <seealso cref="Rentitas.Pool{T}"/></typeparam>
    public interface ISetPool<T> : ISystem where T : class, IComponent
    {
        void SetPool(Pool<T> typedPool);
    }
}