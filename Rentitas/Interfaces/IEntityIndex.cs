using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rentitas
{
    public interface IEntityIndex
    {
        void Activate();
        void Deactivate();
    }

    public abstract class AbstractEntityIndex<TPool, TKey> : IEntityIndex
        where TPool : class, IComponent
    {
        protected readonly Group<TPool> _group;
        protected readonly Func<Entity<TPool>, TPool, TKey> _getKey;

        protected AbstractEntityIndex(
            Group<TPool> group,
            Func<Entity<TPool>, TPool, TKey> getKey)
        {
            _group = group;
            _getKey = getKey;
        }

        public virtual void Activate()
        {
            _group.OnEntityAdded += OnEntityAdded;
            _group.OnEntityRemoved += OnEntityRemoved;
        }

        public virtual void Deactivate()
        {
            _group.OnEntityAdded -= OnEntityAdded;
            _group.OnEntityRemoved -= OnEntityRemoved;
            Clear();
        }

        protected void IndexEntities(Group<TPool> group)
        {
            var entities = group.GetEntities();
            for (int i = 0; i < entities.Length; i++)
            {
                AddEntity(entities[i], null);
            }
        }

        protected void OnEntityAdded(
            Group<TPool> group, Entity<TPool> entity, Type type, TPool component)
        {
            AddEntity(entity, component);
        }

        protected void OnEntityRemoved(
            Group<TPool> group, Entity<TPool> entity, Type type, TPool component)
        {
            RemoveEntity(entity, component);
        }

        protected abstract void AddEntity(Entity<TPool> entity, TPool component);

        protected abstract void RemoveEntity(Entity<TPool> entity, TPool component);

        protected abstract void Clear();

        ~AbstractEntityIndex()
        {
            Deactivate();
        }
    }

    public class PrimaryEntityIndex<TPool, TKey> : AbstractEntityIndex<TPool, TKey>
        where TPool : class, IComponent
    {

        protected readonly Dictionary<TKey, Entity<TPool>> _index;

        public PrimaryEntityIndex(
            Group<TPool> group,
            Func<Entity<TPool>, TPool, TKey> getKey
        ) : base(group, getKey)
        {
            _index = new Dictionary<TKey, Entity<TPool>>();
            Activate();
        }

        public PrimaryEntityIndex(
            Group<TPool> group, Func<Entity<TPool>,
            TPool, TKey> getKey,
            IEqualityComparer<TKey> comparer) : base(group, getKey)
        {
            _index = new Dictionary<TKey, Entity<TPool>>(comparer);
            Activate();
        }

        public override void Activate()
        {
            base.Activate();
            IndexEntities(_group);
        }

        public bool HasEntity(TKey key)
        {
            return _index.ContainsKey(key);
        }

        public Entity<TPool> GetEntity(TKey key)
        {
            var entity = TryGetEntity(key);
            if (entity == null)
            {
                throw new EntityIndexException(
                    "Entity for key '" + key + "' doesn't exist!",
                    "You should check if an entity with that key exists " +
                    "before getting it."
                );
            }

            return entity;
        }

        public Entity<TPool> TryGetEntity(TKey key)
        {
            Entity<TPool> entity;
            _index.TryGetValue(key, out entity);
            return entity;
        }

        protected override void Clear()
        {
            foreach (var entity in _index.Values)
            {
                entity.Release(this);
            }

            _index.Clear();
        }

        protected override void AddEntity(
            Entity<TPool> entity, TPool component)
        {
            var key = _getKey(entity, component);
            if (_index.ContainsKey(key))
            {
                throw new EntityIndexException(
                    "Entity for key '" + key + "' already exists!",
                    "Only one entity for a primary key is allowed.");
            }

            _index.Add(key, entity);
            entity.Retain(this);
        }

        protected override void RemoveEntity(
            Entity<TPool> entity, TPool component)
        {
            // TODO: Improve removing!
//            var key = _index.FirstOrDefault(x => x.Value == entity).Key;
//            _index.Remove(key);
            _index.Remove(_getKey(entity, component));
            entity.Release(this);
        }
    }

    public class ComparablePrimaryIndex<TPool, TKey> : PrimaryEntityIndex<TPool, TKey>
        where TPool : class, IComponent
    {
        private Func<Entity<TPool>, Entity<TPool>, bool> _compareEntities;

        public ComparablePrimaryIndex
            (Group<TPool> @group, 
            Func<Entity<TPool>, Entity<TPool>, bool> compareEntities, 
            Func<Entity<TPool>, TPool, TKey> getKey) : base(@group, getKey)
        {
            _compareEntities = compareEntities;
        }

        public ComparablePrimaryIndex(
            Group<TPool> @group, 
            Func<Entity<TPool>, TPool, TKey> getKey,
            Func<Entity<TPool>, Entity<TPool>, bool> compareEntities,
            IEqualityComparer<TKey> comparer) : base(@group, getKey, comparer)
        {
            _compareEntities = compareEntities;
        }

        protected override void AddEntity(Entity<TPool> entity, TPool component)
        {
            var key = _getKey(entity, component);
            if (HasEntity(key) && _compareEntities(GetEntity(key), entity))
            {
                _index[key].Release(this);
                _index[key] = entity;
                entity.Retain(this);
            } else if (!HasEntity(key))
            {
                _index.Add(key, entity);
                entity.Retain(this);
            }
        }
    }

    public class EntityIndex<TPool, TKey> : AbstractEntityIndex<TPool, TKey>
        where TPool : class, IComponent
    {

        readonly Dictionary<TKey, HashSet<Entity<TPool>>> _index;

        public EntityIndex(Group<TPool> group, Func<Entity<TPool>, TPool, TKey> getKey) :
            base(group, getKey)
        {
            _index = new Dictionary<TKey, HashSet<Entity<TPool>>>();
            Activate();
        }

        public EntityIndex(
            Group<TPool> group,
            Func<Entity<TPool>, TPool, TKey> getKey,
            IEqualityComparer<TKey> comparer) : base(group, getKey)
        {
            _index = new Dictionary<TKey, HashSet<Entity<TPool>>>(comparer);
            Activate();
        }

        public override void Activate()
        {
            base.Activate();
            IndexEntities(_group);
        }

        public bool HasEntities(TKey key)
        {
            return _index.ContainsKey(key);
        }

        public HashSet<Entity<TPool>> GetEntities(TKey key)
        {
            HashSet<Entity<TPool>> entities;
            if (!_index.TryGetValue(key, out entities))
            {
                entities = new HashSet<Entity<TPool>>(EntityEqualityComparer<TPool>.Comparer);
                _index.Add(key, entities);
            }

            return entities;
        }

        protected override void Clear()
        {
            foreach (var entities in _index.Values)
            {
                foreach (var entity in entities)
                {
                    entity.Release(this);
                }
            }

            _index.Clear();
        }

        protected override void AddEntity(Entity<TPool> entity, TPool component)
        {
            GetEntities(_getKey(entity, component)).Add(entity);
            entity.Retain(this);
        }

        protected override void RemoveEntity(
            Entity<TPool> entity, TPool component)
        {
            GetEntities(_getKey(entity, component)).Remove(entity);
            entity.Release(this);
        }
    }

    public class EntityIndexException : RentitasException
    {
        public EntityIndexException(string message, string hint) :
            base(message, hint)
        {
        }
    }
}
