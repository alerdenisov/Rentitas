using System;

namespace Rentitas
{
    public partial class Entity<T> where T : class, IComponent
    {
        /// Occurs when a component gets added. All event handlers will be removed when the entity gets destroyed by the pool.
        public event EntityChanged OnComponentAdded;

        /// Occurs when a component gets removed. All event handlers will be removed when the entity gets destroyed by the pool.
        public event EntityChanged OnComponentRemoved;

        /// Occurs when a component gets replaced. All event handlers will be removed when the entity gets destroyed by the pool.
        public event ComponentReplaced OnComponentReplaced;

        /// Occurs when an entity gets released and is not retained anymore. All event handlers will be removed when the entity gets destroyed by the pool.
        public event EntityReleased OnEntityReleased;


        public delegate void EntityChanged(Entity<T> entity, Type type, T component);
        public delegate void ComponentReplaced(Entity<T> entity, Type type, T previousComponent, T newComponent);
        public delegate void EntityReleased(Entity<T> entity);

    }
}