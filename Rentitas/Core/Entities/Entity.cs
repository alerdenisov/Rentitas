using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rentitas.Caching;
using UnityEngine;

namespace Rentitas
{
    public partial class Entity<T> : IEntity where T : class, IComponent
    {

        public int Id => _creationIndex;
        public Dictionary<Type, Stack<T>> ComponentsPool => _componentsPool;
        public bool IsEnabled => _isEnabled;
        public int RetainCount => owners.Count;
        public PoolMeta PoolMeta => _poolMeta;
        public int TotalComponents => PoolMeta.TotalComponents;

        /// Use pool.CreateEntity() to create a new entity and pool.DestroyEntity() to destroy it.
        public Entity(Dictionary<Type, Stack<T>> componentsPool, PoolMeta meta)
        {
            _poolMeta = meta;
            _componentsPool = componentsPool;
            _totalComponents = meta.TotalComponents;
            _components = new Dictionary<Type, T>(_totalComponents);

            foreach (var kv in _componentsPool) _components.Add(kv.Key, null);
        }

        public T[] GetComponents()
        {
            if (_componentsCache == null)
            {
                var components = RentitasCache.GetComponentList<T>();

                foreach (var componentType in _poolMeta.ComponentTypes)
                {
                    if(Has(componentType))
                        components.Add(Get(componentType));
                }

                _componentsCache = components.ToArray();
            }

            return _componentsCache;
        }


        public Type[] GetComponentsTypes()
        {
            if (_componentTypesCache == null)
            {
                var types = RentitasCache.GetTypeHashSet();

                foreach (var componentType in _poolMeta.ComponentTypes)
                {
                    if (Has(componentType))
                        types.Add(componentType);


                }

                _componentTypesCache = types.ToArray();

                RentitasCache.PushTypeHashSet(types);
            }

            return _componentTypesCache;
        }

        public Entity<T> Add<T2>(Func<T2,T2> mod = null) where T2 : T, new()
        {
            var comp = CreateComponent<T2>();
            mod?.Invoke(comp);
            AddInstance(comp);

            return this;
        } 

        public T Get(Type componentType)
        {
            if (!Has(componentType))
                throw new EntityDoesNotHaveComponentException<T>(
                    componentType,
                    "Cannot get component '" + PoolMeta.ComponentNames[componentType] + "' from " + this + "!",
                    "You should check if an entity has the component before getting it."
                    );

            return _components[componentType];
        }

        public T2 Get<T2>() where T2 : T, new()
        {
            return (T2) Get(typeof (T2));
        }

        public bool Has<T2>() where T2 : T
        {
            return Has(typeof(T2));
        }

        public Entity<T> Remove<T2>() where T2 : T
        {
            return Remove(typeof(T2));
        }

        public Entity<T> Replace<T2>(Func<T2, T2> mod = null) where T2 : T, new()
        {
            var comp = CreateComponent<T2>();
            mod?.Invoke(comp);
            ReplaceInstance(comp);
            return this;
        }

        public Entity<T> ReplaceInstance<T2>(T2 component) where T2 : T, new()
        {
            return ReplaceInstance(typeof(T2), component);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_toStringCache))
            {
                // TODO: Optimize active components!
                _toStringCache = string.Format(
                    "[{0}] Entity [Owners: {1}] [Components: {2}] {3}",
                    Id, owners.Count, GetComponents().Length,
                    string.Join(", ", GetComponentsTypes().Select(t => PoolMeta.ComponentNames[t]).ToArray()));
            }

            return _toStringCache;
        }

        public bool Is<T2>() where T2 : T, IFlag
        {
            return Has<T2>();
        }

        public Entity<T> Toggle<T2>() where T2 : T, IFlag, new()
        {
            var current = Is<T2>();
            return Toggle<T2>(!current);
        }
        public Entity<T> Toggle<T2>(bool flag) where T2 : T, IFlag, new()
        {
            var current = Is<T2>();
            if (current == flag) return this;

            if (flag) Add<T2>();
            else Remove<T2>();

            return this;

        }
    }
}
