using System;
using System.Collections.Generic;

namespace Rentitas
{
    public partial class Pool<T> where T : class, IComponent
    {
        readonly Dictionary<Type, Stack<T>>
            _componentPools;

        readonly int _totalComponents;
        readonly HashSet<Entity<T>> _entities = new HashSet<Entity<T>>(EntityEqualityComparer<T>.Comparer);
        readonly Stack<Entity<T>> _reusableEntities = new Stack<Entity<T>>();
        readonly HashSet<Entity<T>> _retainedEntities = new HashSet<Entity<T>>(EntityEqualityComparer<T>.Comparer);
        readonly Dictionary<Type, Group<T>> _singletons = new Dictionary<Type, Group<T>>();

        int _creationIndex;
        Entity<T>[] _entitiesCache;

        // Cache delegates to avoid gc allocations
        Entity<T>.EntityChanged _cachedUpdateGroupsComponentAddedOrRemoved;
        Entity<T>.ComponentReplaced _cachedUpdateGroupsComponentReplaced;
        Entity<T>.EntityReleased _cachedOnEntityReleased;
        readonly PoolMeta _metaData;

        readonly Dictionary<IMatcher, Group<T>> _groups = new Dictionary<IMatcher, Group<T>>();
        Dictionary<Type, List<Group<T>>> _groupsForTypes;
    }
}