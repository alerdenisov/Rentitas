using System;
using System.Collections.Generic;
using Rentitas.Caching;

namespace Rentitas
{
    public partial class Matcher
    {
        public Type[] MergeTypes()
        {
            var TypesList = RentitasCache.GetTypeList();

            if (_allOfTypes != null)
            {
                TypesList.AddRange(_allOfTypes);
            }
            if (_anyOfTypes != null)
            {
                TypesList.AddRange(_anyOfTypes);
            }
            if (_noneOfTypes != null)
            {
                TypesList.AddRange(_noneOfTypes);
            }

            var mergedTypes = DistinctTypes(TypesList);

            RentitasCache.PushTypeList(TypesList);

            return mergedTypes;
        }

        private static Type[] DistinctTypes(IEnumerable<Type> types)
        {
            var typesSet = RentitasCache.GetTypeHashSet();

            foreach (var index in types)
            {
                typesSet.Add(index);
            }
            var uniqueTypes = new Type[typesSet.Count];
            typesSet.CopyTo(uniqueTypes);
            //Array.Sort(uniqueTypes);

            RentitasCache.PushTypeHashSet(typesSet);

            return uniqueTypes;
        }

        private static Type[] MergeTypes(IList<IMatcher> matchers)
        {
            var types = new Type[matchers.Count];
            for (int i = 0; i < matchers.Count; i++)
            {
                var matcher = matchers[i];
                if (matcher.Types.Length != 1)
                {
                    throw new MatcherException(matcher);
                }
                types[i] = matcher.Types[0];
            }

            return types;
        }
    }
}