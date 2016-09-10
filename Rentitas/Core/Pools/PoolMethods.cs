using System;

namespace Rentitas
{
    public partial class Pool
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
    }
}