using System.Collections.Generic;

namespace Rentitas
{
    public class EntityEqualityComparer<T> : IEqualityComparer<Entity<T>> where T : class, IComponent
    {
        public static readonly EntityEqualityComparer<T> Comparer = new EntityEqualityComparer<T>();

        public bool Equals(Entity<T> x, Entity<T> y)
        {
            return x == y;
        }

        public int GetHashCode(Entity<T> obj)
        {
            return obj.Id;
        }
    }

}