using System;
using System.Collections.Generic;

namespace Rentitas
{
    public partial class Entity<T> where T : class, IComponent
    {
        internal int _creationIndex;
        internal bool _isEnabled = true;

        private PoolMeta _poolMeta;
        private string _toStringCache;
        private readonly Dictionary<Type, Stack<T>> _componentsPool;
        private readonly int _totalComponents;
        private readonly Dictionary<Type, T> _components;

        private Type[] _componentTypesCache;
        private T[] _componentsCache;
    }
}