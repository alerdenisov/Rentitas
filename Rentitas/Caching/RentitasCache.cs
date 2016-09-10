using System;
using System.Collections.Generic;

namespace Rentitas.Caching
{
    public static class RentitasCache
    {
        static ObjectCache _cache = new ObjectCache();

        public static List<Group.GroupChanged> GetGroupChangedList() { return _cache.Get<List<Group.GroupChanged>>(); }
        public static void PushGroupChangedList(List<Group.GroupChanged> list) { list.Clear(); _cache.Push(list); }

        public static List<Type> GetTypeList() { return _cache.Get<List<Type>>(); }
        public static void PushTypeList(List<Type> list) { list.Clear(); _cache.Push(list); }

        public static HashSet<Type> GetTypeHashSet() { return _cache.Get<HashSet<Type>>(); }
        public static void PushTypeHashSet(HashSet<Type> hash) { hash.Clear(); _cache.Push(hash); }

        public static List<IComponent> GetIComponentList() { return _cache.Get<List<IComponent>>(); }
        public static void PushIComponentList(List<IComponent> list) { list.Clear(); _cache.Push(list); } 
    }
}