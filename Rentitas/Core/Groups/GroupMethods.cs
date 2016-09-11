using System;
using System.Linq;

namespace Rentitas
{
    public partial class Group<T> where T : class, IComponent
    {
        /// This is used by the pool to manage the group.
        public void HandleEntitySilently(Entity<T> entity)
        {
            if (_matcher.Matches(entity))
                AddEntitySilently(entity);
            else
                RemoveEntitySilently(entity);
        }

        /// This is used by the pool to manage the group.
        public void HandleEntity(Entity<T> entity, Type type, T component)
        {
            if (_matcher.Matches(entity))
                AddEntity(entity, type, component);
            else
                RemoveEntity(entity, type, component);
        }

        /// This is used by the pool to manage the group.
        public void UpdateEntity(Entity<T> entity, Type type, T previousComponent, T newComponent)
        {
            if (_entities.Contains(entity))
            {
                OnEntityRemoved?.Invoke(this, entity, type, previousComponent);
                OnEntityAdded?.Invoke(this, entity, type, newComponent);
                OnEntityUpdated?.Invoke(this, entity, type, previousComponent, newComponent);
            }
        }

        /// This is called by pool.Reset() and pool.ClearGroups() to remove all event handlers.
        /// This is useful when you want to soft-restart your application.
        public void RemoveAllEventHandlers()
        {
            OnEntityAdded = null;
            OnEntityRemoved = null;
            OnEntityUpdated = null;
        }

        internal GroupChanged HandleEntity(Entity<T> entity)
        {
            return _matcher.Matches(entity)
                       ? (AddEntitySilently(entity) ? OnEntityAdded : null)
                       : (RemoveEntitySilently(entity) ? OnEntityRemoved : null);
        }

        bool AddEntitySilently(Entity<T> entity)
        {
            var added = _entities.Add(entity);
            if (added)
            {
                _entitiesCache = null;
                _singleEntityCache = null;
                entity.Retain(this);
            }

            return added;
        }

        void AddEntity(Entity<T> entity, Type type, T component)
        {
            if (AddEntitySilently(entity))
            {
                OnEntityAdded?.Invoke(this, entity, type, component);
            }
        }

        bool RemoveEntitySilently(Entity<T> entity)
        {
            var removed = _entities.Remove(entity);
            if (removed)
            {
                _entitiesCache = null;
                _singleEntityCache = null;
                entity.Release(this);
            }

            return removed;
        }

        void RemoveEntity(Entity<T> entity, Type type, T component)
        {
            var removed = _entities.Remove(entity);
            if (removed)
            {
                _entitiesCache = null;
                _singleEntityCache = null;
                OnEntityRemoved?.Invoke(this, entity, type, component);
                entity.Release(this);
            }
        }

        /// Determines whether this group has the specified entity.
        public bool ContainsEntity(Entity<T> entity)
        {
            return _entities.Contains(entity);
        }

        /// Returns all entities which are currently in this group.
        public Entity<T>[] GetEntities()
        {
            if (_entitiesCache == null)
            {
                _entitiesCache = new Entity<T>[_entities.Count];
                _entities.CopyTo(_entitiesCache);
            }

            return _entitiesCache;
        }

        /// Returns the only entity in this group. It will return null if the group is empty.
        /// It will throw an exception if the group has more than one entity.
        public Entity<T> GetSingleEntity()
        {
            if (_singleEntityCache == null)
            {
                var c = _entities.Count;
                if (c == 1)
                {
                    using (var enumerator = _entities.GetEnumerator())
                    {
                        enumerator.MoveNext();
                        _singleEntityCache = enumerator.Current;
                    }
                }
                else if (c == 0)
                {
                    return null;
                }
                else
                {
                    throw new GroupSingleEntityException<T>(this);
                }
            }

            return _singleEntityCache;
        }
    }


    public class GroupSingleEntityException<T> : RentitasException where T : class, IComponent
    {
        public GroupSingleEntityException(Group<T> group) :
            base("Cannot get the single entity from " + group + "!\nGroup contains " + group.Count + " entities:",
                string.Join("\n", group.GetEntities().Select(e => e.ToString()).ToArray()))
        {
        }
    }
}