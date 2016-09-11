using System.Collections.Generic;

namespace Rentitas
{
    public partial class Group<T> where T : class, IComponent
    {
        readonly IMatcher              _matcher;
        readonly HashSet<Entity<T>>    _entities = new HashSet<Entity<T>>(EntityEqualityComparer<T>.Comparer);
        Entity<T>[]                    _entitiesCache;
        Entity<T>                      _singleEntityCache;
        string                         _toStringCache;
    }
}