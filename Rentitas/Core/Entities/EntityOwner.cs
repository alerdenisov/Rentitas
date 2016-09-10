using System.Collections.Generic;

namespace Rentitas
{
    public partial class Entity
    {
        public readonly HashSet<object> owners = new HashSet<object>();

        public Entity Retain(object owner)
        {
            if (!owners.Add(owner))
            {
                throw new EntityIsAlreadyRetainedByOwnerException(this, owner);
            }

            _toStringCache = null;

            return this;
        }

        public void Release(object owner)
        {
            if (!owners.Remove(owner))
            {
                throw new EntityIsNotRetainedByOwnerException(this, owner);
            }

            if (owners.Count == 0)
            {
                _toStringCache = null;
                OnEntityReleased?.Invoke(this);
            }
        }
    }
}