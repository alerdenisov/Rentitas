using System;
using Rentitas.Caching;

namespace Rentitas
{
    public partial class Pool<T> where T : class, IComponent
    {
        /// Occurs when an entity gets created.
        public event PoolChanged OnEntityCreated;

        /// Occurs when an entity will be destroyed.
        public event PoolChanged OnEntityWillBeDestroyed;

        /// Occurs when an entity got destroyed.
        public event PoolChanged OnEntityDestroyed;

        /// Occurs when a group gets created for the first time.
        public event GroupChanged OnGroupCreated;

        /// Occurs when a group gets cleared.
        public event GroupChanged OnGroupCleared;

        public delegate void PoolChanged(Pool<T> pool, Entity<T> entity);
        public delegate void GroupChanged(Pool<T> pool, Group<T> group);


        void UpdateGroupsComponentAddedOrRemoved(Entity<T> entity, Type type, T component)
        {
            var groups = _groupsForTypes[type];
            if (groups != null)
            {
                var events = RentitasCache.GetGroupChangedList<T>();

                for (int i = 0; i < groups.Count; i++)
                {
                    events.Add(groups[i].HandleEntity(entity));
                }

                for (int i = 0; i < events.Count; i++)
                {
                    var groupChangedEvent = events[i];
                    groupChangedEvent?.Invoke(groups[i], entity, type, component);
                }

                RentitasCache.PushGroupChangedList<T>(events);
            }
        }

        void UpdateGroupsComponentReplaced(Entity<T> entity, Type type, T previousComponent, T newComponent)
        {
            var groups = _groupsForTypes[type];
            if (groups != null)
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    groups[i].UpdateEntity(entity, type, previousComponent, newComponent);
                }
            }
        }

        void OnEntityReleased(Entity<T> entity)
        {
            if (entity._isEnabled)
            {
                throw new EntityIsNotDestroyedException<T>("Cannot release " + entity + "!");
            }
            entity.RemoveAllOnEntityReleasedHandlers();
            _retainedEntities.Remove(entity);
            _reusableEntities.Push(entity);
        }

    }
}