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

        public static T GetGeneric<T>() where T : new() { return _cache.Get<T>(); }
        public static void PushGeneric<T>(T instance) where T : new() { _cache.Push(instance); }

        public static List<Type> GetTypeList() { return _cache.Get<List<Type>>(); }
        public static void PushTypeList(List<Type> list) { list.Clear(); _cache.Push(list); }

        public static HashSet<Type> GetTypeHashSet() { return _cache.Get<HashSet<Type>>(); }
        public static void PushTypeHashSet(HashSet<Type> hash) { hash.Clear(); _cache.Push(hash); }

        public static HashSet<T> GetGenericHashSet<T>() { return _cache.Get<HashSet<T>>(); }
        public static void PushGenericHashSet<T>(HashSet<T> hash) { hash.Clear(); _cache.Push(hash); }

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