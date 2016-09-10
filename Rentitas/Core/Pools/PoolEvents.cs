using System;
using Entitas;
using Rentitas.Caching;

namespace Rentitas
{
    public partial class Pool 
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

        public delegate void PoolChanged(Pool pool, Entity entity);
        public delegate void GroupChanged(Pool pool, Group group);


        void updateGroupsComponentAddedOrRemoved(Entity entity, Type type, IComponent component)
        {
            var groups = _groupsForTypes[type];
            if (groups != null)
            {
                var events = RentitasCache.GetGroupChangedList();

                for (int i = 0; i < groups.Count; i++)
                {
                    events.Add(groups[i].HandleEntity(entity));
                }

                for (int i = 0; i < events.Count; i++)
                {
                    var groupChangedEvent = events[i];
                    groupChangedEvent?.Invoke(groups[i], entity, type, component);
                }

                RentitasCache.PushGroupChangedList(events);
            }
        }

        void updateGroupsComponentReplaced(Entity entity, Type type, IComponent previousComponent, IComponent newComponent)
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

        void onEntityReleased(Entity entity)
        {
            if (entity._isEnabled)
            {
                throw new EntityIsNotDestroyedException("Cannot release " + entity + "!");
            }
            entity.RemoveAllOnEntityReleasedHandlers();
            _retainedEntities.Remove(entity);
            _reusableEntities.Push(entity);
        }

    }
}