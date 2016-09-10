﻿using System;
using System.Collections.Generic;

namespace Rentitas
{
    public partial class Entity
    {
        internal int        _creationIndex;
        internal bool       _isEnabled = true;

        private PoolMeta                                            _poolMeta;
        private string                                              _toStringCache;
        private readonly Dictionary<Type, Stack<IComponent>>        _componentsPool;
        private readonly int                                        _totalComponents;
        private readonly Dictionary<Type, IComponent>               _components;

        private Type[]              _componentTypesCache;
        private IComponent[]        _componentsCache;
    }
}