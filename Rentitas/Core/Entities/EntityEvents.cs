using System;

namespace Rentitas
{
    public partial class Entity
    {
        /// Occurs when a component gets added. All event handlers will be removed when the entity gets destroyed by the pool.
        public event EntityChanged OnComponentAdded;

        /// Occurs when a component gets removed. All event handlers will be removed when the entity gets destroyed by the pool.
        public event EntityChanged OnComponentRemoved;

        /// Occurs when a component gets replaced. All event handlers will be removed when the entity gets destroyed by the pool.
        public event ComponentReplaced OnComponentReplaced;

        /// Occurs when an entity gets released and is not retained anymore. All event handlers will be removed when the entity gets destroyed by the pool.
        public event EntityReleased OnEntityReleased;


        public delegate void EntityChanged(Entity entity, Type type, IComponent component);
        public delegate void ComponentReplaced(Entity entity, Type type, IComponent previousComponent, IComponent newComponent);
        public delegate void EntityReleased(Entity entity);

    }
}