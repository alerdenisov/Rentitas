using System;

namespace Rentitas
{
    public interface IPool
    {
        Type PoolType { get; }
        string PoolName { get; }
    }
}