using System;
using System.Collections.Generic;

namespace Rentitas
{
    public partial class Pool
    {
        readonly Dictionary<Type, Stack<IComponent>> 
                                        _componentPools;

        readonly int                    _totalComponents;
        readonly HashSet<Entity>        _entities                   = new HashSet<Entity>(EntityEqualityComparer.Comparer);
        readonly Stack<Entity>          _reusableEntities           = new Stack<Entity>();
        readonly HashSet<Entity>        _retainedEntities           = new HashSet<Entity>(EntityEqualityComparer.Comparer);

        int                             _creationIndex;
        Entity[]                        _entitiesCache;

        // Cache delegates to avoid gc allocations
        Entity.EntityChanged            _cachedUpdateGroupsComponentAddedOrRemoved;
        Entity.ComponentReplaced        _cachedUpdateGroupsComponentReplaced;
        Entity.EntityReleased           _cachedOnEntityReleased;
        readonly PoolMeta               _metaData;

        readonly Dictionary<IMatcher, Group> _groups = new Dictionary<IMatcher, Group>();
        Dictionary<Type, List<Group>>   _groupsForTypes;
    }
}