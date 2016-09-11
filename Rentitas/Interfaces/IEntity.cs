using System;

namespace Rentitas
{
    public interface IEntity
    {
        bool Has(params Type[] types);
        bool HasAny(params Type[] types);
    }
}