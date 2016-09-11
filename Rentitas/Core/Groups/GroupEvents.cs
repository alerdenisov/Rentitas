using System;

namespace Rentitas
{
    public partial class Group<T> where T : class, IComponent
    {
        /// Occurs when an entity gets added.
        public event GroupChanged OnEntityAdded;

        /// Occurs when an entity gets removed.
        public event GroupChanged OnEntityRemoved;

        /// Occurs when a component of an entity in the group gets replaced.
        public event GroupUpdated OnEntityUpdated;

        public delegate void GroupChanged(Group<T> group, Entity<T> entity, Type type, T component);
        public delegate void GroupUpdated(Group<T> group, Entity<T> entity, Type type, T previousComponent, T newComponent);
    }
}