using System.Collections.Generic;

namespace Rentitas
{
    public partial class Entity<T> where T : class, IComponent
    {
        public readonly HashSet<object> owners = new HashSet<object>();

        public Entity<T> Retain(object owner)
        {
            if (!owners.Add(owner))
            {
                throw new EntityIsAlreadyRetainedByOwnerException<T>(this, owner);
            }

            _toStringCache = null;

            return this;
        }

        public void Release(object owner)
        {
            if (!owners.Remove(owner))
            {
                throw new EntityIsNotRetainedByOwnerException<T>(this, owner);
            }

            if (owners.Count == 0)
            {
                _toStringCache = null;
                OnEntityReleased?.Invoke(this);
            }
        }
    }
}