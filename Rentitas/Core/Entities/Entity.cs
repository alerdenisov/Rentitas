using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rentitas.Caching;
using UnityEngine;

namespace Rentitas
{
    public partial class Entity
    {

        public int Id => _creationIndex;
        public Dictionary<Type, Stack<IComponent>> ComponentsPool => _componentsPool;
        public bool IsEnabled => _isEnabled;
        public int RetainCount => owners.Count;
        public PoolMeta PoolMeta => _poolMeta;
        public int TotalComponents => PoolMeta.TotalComponents;

        /// Use pool.CreateEntity() to create a new entity and pool.DestroyEntity() to destroy it.
        public Entity(Dictionary<Type, Stack<IComponent>> componentsPool, PoolMeta meta)
        {
            _poolMeta = meta;
            _componentsPool = componentsPool;
            _totalComponents = meta.TotalComponents;
            _components = new Dictionary<Type, IComponent>(_totalComponents);

            foreach (var kv in _componentsPool) _components.Add(kv.Key, null);
        }

        public IComponent[] GetComponents()
        {
            if (_componentsCache == null)
            {
                var components = RentitasCache.GetIComponentList();

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

        public Entity Add<T>(Func<T,T> mod = null) where T : IComponent, new()
        {
            var comp = CreateComponent<T>();
            mod?.Invoke(comp);
            AddInstance(comp);

            return this;
        } 

        public IComponent Get(Type componentType)
        {
            if (!Has(componentType))
                throw new EntityDoesNotHaveComponentException(
                    componentType,
                    "Cannot get component '" + PoolMeta.ComponentNames[componentType] + "' from " + this + "!",
                    "You should check if an entity has the component before getting it."
                    );

            return _components[componentType];
        }

        public T Get<T>() where T : IComponent
        {
            return (T) Get(typeof (T));
        }

        public bool Has<T>() where T : IComponent
        {
            return Has(typeof(T));
        }

        public Entity Remove<T>() where T : IComponent
        {
            var t = typeof (T);
            return Remove(typeof(T));
        }

        public Entity Replace<T>(Func<T, T> mod = null) where T : IComponent, new()
        {
            var comp = CreateComponent<T>();
            mod?.Invoke(comp);
            ReplaceInstance(comp);
            return this;
        }

        public Entity ReplaceInstance<T>(T component) where T : IComponent
        {
            return ReplaceInstance(typeof(T), component);
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
    }
}
