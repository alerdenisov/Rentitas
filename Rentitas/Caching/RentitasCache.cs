using System;
using System.Collections.Generic;

namespace Rentitas.Caching
{
    public static class RentitasCache
    {
        static ObjectCache _cache = new ObjectCache();

        public static List<Group<T>.GroupChanged> GetGroupChangedList<T>() 
            where T : class, IComponent
        {
            return _cache.Get<List<Group<T>.GroupChanged>>();
        }

        public static void PushGroupChangedList<T>(List<Group<T>.GroupChanged> list) 
            where T : class, IComponent
        {
            list.Clear();
            _cache.Push(list);
        }

        public static List<Type> GetTypeList() { return _cache.Get<List<Type>>(); }
        public static void PushTypeList(List<Type> list) { list.Clear(); _cache.Push(list); }

        public static HashSet<Type> GetTypeHashSet() { return _cache.Get<HashSet<Type>>(); }
        public static void PushTypeHashSet(HashSet<Type> hash) { hash.Clear(); _cache.Push(hash); }

        public static List<T> GetComponentList<T>()
            where T : IComponent
        {
            return _cache.Get<List<T>>();
        }

        public static void PushComponentList<T>(List<T> list)
            where T : IComponent
        {
            list.Clear();
            _cache.Push(list);
        } 
    }
}