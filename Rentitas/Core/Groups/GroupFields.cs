using System.Collections.Generic;

namespace Rentitas
{
    public partial class Group
    {
        readonly IMatcher           _matcher;
        readonly HashSet<Entity>    _entities = new HashSet<Entity>(EntityEqualityComparer.Comparer);
        Entity[]                    _entitiesCache;
        Entity                      _singleEntityCache;
        string                      _toStringCache;
    }
}