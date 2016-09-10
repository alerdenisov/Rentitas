using System;

namespace Rentitas
{
    public class RentitasException : Exception
    {
        public RentitasException(string message, string hint) : base(hint != null ? (message + "\n" + hint) : message) { }
    }

    public class MatcherException : RentitasException
    {
        public MatcherException(IMatcher matcher) : base("matcher.indices.Length must be 1 but was " + matcher.Types.Length, null)
        {
        }
    }

    public class EntityException : RentitasException
    {
        public EntityException(Type index, string message, string hint) : base(hint, message + " at index " + index) { }
    }
    public class EntityAlreadyHasComponentException : EntityException
    {
        public EntityAlreadyHasComponentException(Type index, string message, string hint) :
            base(index, message + "\nEntity already has a component", hint)
        {
        }
    }

    public class EntityDoesNotHaveComponentException : EntityException
    {
        public EntityDoesNotHaveComponentException(Type index, string message, string hint) :
            base(index, message + "\nEntity does not have a component", hint)
        {
        }
    }

    public class EntityIsNotEnabledException : EntityException
    {
        public EntityIsNotEnabledException(string message) :
            base(null, message + "\nEntity is not enabled!", "The entity has already been destroyed. You cannot modify destroyed entities.")
        {
        }
    }

    public class EntityIsAlreadyRetainedByOwnerException : EntityException
    {
        public EntityIsAlreadyRetainedByOwnerException(Entity entity, object owner) :
            base(null, "'" + owner + "' cannot retain " + entity + "!\nEntity is already retained by this object!",
                "The entity must be released by this object first.")
        {
        }
    }

    public class EntityIsNotRetainedByOwnerException : EntityException
    {
        public EntityIsNotRetainedByOwnerException(Entity entity, object owner) :
            base(null, "'" + owner + "' cannot release " + entity + "!\nEntity is not retained by this object!",
                "An entity can only be released from objects that retain it.")
        {
        }
    }

    public class PoolDoesNotContainEntityException : RentitasException
    {
        public PoolDoesNotContainEntityException(string message, string hint) :
            base(message + "\nPool does not contain entity!", hint)
        {
        }
    }

    public class EntityIsNotDestroyedException : RentitasException
    {
        public EntityIsNotDestroyedException(string message) :
            base(message + "\nEntity is not destroyed yet!",
                "Did you manually call entity.Release(pool) yourself? If so, please don't :)")
        {
        }
    }

    public class PoolStillHasRetainedEntitiesException : RentitasException
    {
        public PoolStillHasRetainedEntitiesException(Pool pool) :
            base("'" + pool + "' detected retained entities although all entities got destroyed!",
                "Did you release all entities? Try calling pool.ClearGroups() and systems.ClearReactiveSystems() before calling pool.DestroyAllEntities() to avoid memory leaks.")
        {
        }
    }

    public class PoolMetaDataException : RentitasException
    {
        public PoolMetaDataException(Pool pool, PoolMeta poolMetaData) :
            base("Invalid PoolMetaData for '" + pool + "'!\nExpected " + pool.TotalComponents + " componentName(s) but got " + poolMetaData.TotalComponents, null)
        {
        }
    }

    public class PoolEntityIndexDoesNotExistException : RentitasException
    {
        public PoolEntityIndexDoesNotExistException(Pool pool, string name) :
            base("Cannot get EntityIndex '" + name + "' from pool '" + pool + "'!", "No EntityIndex with this name has been added.")
        {
        }
    }

    public class PoolEntityIndexDoesAlreadyExistException : RentitasException
    {
        public PoolEntityIndexDoesAlreadyExistException(Pool pool, string name) :
            base("Cannot add EntityIndex '" + name + "' to pool '" + pool + "'!", "An EntityIndex with this name has already been added.")
        {
        }
    }
}