using System;

namespace Rentitas
{
    public partial class Group
    {
        /// Occurs when an entity gets added.
        public event GroupChanged OnEntityAdded;

        /// Occurs when an entity gets removed.
        public event GroupChanged OnEntityRemoved;

        /// Occurs when a component of an entity in the group gets replaced.
        public event GroupUpdated OnEntityUpdated;

        public delegate void GroupChanged(Group group, Entity entity, Type type, IComponent component);
        public delegate void GroupUpdated(Group group, Entity entity, Type type, IComponent previousComponent, IComponent newComponent);
    }
}