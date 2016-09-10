using System.Collections.Generic;

namespace Rentitas
{
    public class EntityEqualityComparer : IEqualityComparer<Entity>
    {
        public static readonly EntityEqualityComparer Comparer = new EntityEqualityComparer();

        public bool Equals(Entity x, Entity y)
        {
            return x == y;
        }

        public int GetHashCode(Entity obj)
        {
            return obj.Id;
        }
    }

}