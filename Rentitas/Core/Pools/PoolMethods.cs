using System;

namespace Rentitas
{
    public partial class Pool<T> where T : class, IComponent
    {
        public void ClearComponentPool(Type t)
        {
            _componentPools[t].Clear();
        }
        public void ClearComponentPools()
        {
            for (int i = 0; i < _metaData.ComponentTypes.Length; i++)
                ClearComponentPool(_metaData.ComponentTypes[i]);
        }

        public T2 CreateComponent<T2>() where T2 : T, new()
        {
            var pool = ComponentPools[typeof (T2)];
            if (pool.Count > 0)
                return (T2)pool.Pop();

            return new T2();
        }

    }
}