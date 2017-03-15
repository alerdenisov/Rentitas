using System;
using System.Collections.Generic;

namespace Rentitas
{
    public class Pools
    {
        private List<IPool> _poolRepository;
        private Dictionary<string, int> _namedPools;
        private Dictionary<Type, List<int>> _typedPools;


        public Pools()
        {
            _poolRepository = new List<IPool>();
            _namedPools = new Dictionary<string, int>();
            _typedPools = new Dictionary<Type, List<int>>();
        }

        public void Register(IPool pool)
        {
            var type = pool.PoolType;
            var name = pool.PoolName;

            var index = _poolRepository.Count;
            _poolRepository.Add(pool);
            if(!string.IsNullOrEmpty(name))
                _namedPools.Add(name, index);

            if(!_typedPools.ContainsKey(type))
                _typedPools.Add(type, new List<int>());

            _typedPools[type].Add(index);
        }

        public Pool<T> Get<T>(string name) where T : class, IComponent
        {
            return (Pool<T>) Get(name);
        }

        public IPool Get(string name) 
        {
            return _namedPools.ContainsKey(name) ? GetPool(_namedPools[name]) : null;
        }

        public bool Has(Type type)
        {
            return _typedPools.ContainsKey(type);
        }

        public bool Has<T>()
        {
            return Has(typeof (T));
        }

        public Pool<T> Get<T>() where T : class, IComponent
        {
            var type = typeof (T);
            if (!_typedPools.ContainsKey(type))
                throw new UnknownTypeOfPoolException(type);

            if (_typedPools[type] == null || _typedPools[type].Count == 0)
                throw new NonRegisteredPoolsException(type);

            return (Pool<T>)GetPool(_typedPools[type][0]);
        }

        private IPool GetPool(int index)
        {
            if (_poolRepository.Count <= index)
                throw new IncorrectIndexOfPool(index);

            return _poolRepository[index];
        }

        public IPool[] GetPools()
        {
            return _poolRepository.ToArray();
        }
    }
}