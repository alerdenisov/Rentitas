using System;

namespace Rentitas
{
    public partial class Matcher : IAllOfMatcher, IAnyOfMatcher, INoneOfMatcher
    {
        public Type[] Types
        {
            get
            {
                if (_types == null)
                {
                    _types = MergeTypes();
                }

                return _types;
            }
        }

        public Type[] AllOfTypes => _allOfTypes;
        public Type[] AnyOfTypes => _anyOfTypes;
        public Type[] NoneOfTypes => _noneOfTypes;

        Matcher()
        {
        }

        public bool Matches(Entity entity)
        {
            var matchesAllOf =  _allOfTypes  == null ||  entity.Has(_allOfTypes);
            var matchesAnyOf =  _anyOfTypes  == null ||  entity.HasAny(_anyOfTypes);
            var matchesNoneOf = _noneOfTypes == null || !entity.HasAny(_noneOfTypes);

            return matchesAllOf && matchesAnyOf && matchesNoneOf;
        }

        IAnyOfMatcher IAllOfMatcher.AnyOf(params Type[] types)
        {
            _anyOfTypes = DistinctTypes(types);
            _types = null;
            return this;
        }

        IAnyOfMatcher IAllOfMatcher.AnyOf(params IMatcher[] matchers)
        {
            return ((IAllOfMatcher)this).AnyOf(MergeTypes(matchers));
        }

        public INoneOfMatcher NoneOf(params Type[] types)
        {
            _noneOfTypes = DistinctTypes(types);
            _types = null;
            return this;
        }

        public INoneOfMatcher NoneOf(params IMatcher[] matchers)
        {
            return NoneOf(MergeTypes(matchers));
        }


        public static IAllOfMatcher AllOf(params Type[] types)
        {
            var matcher = new Matcher();
            matcher._allOfTypes = DistinctTypes(types);
            return matcher;
        }

        public static IAllOfMatcher AllOf(params IMatcher[] matchers)
        {
            var allOfMatcher = (Matcher)AllOf(MergeTypes(matchers));
//            setComponentNames(allOfMatcher, matchers);
            return allOfMatcher;
        }

        public static IAnyOfMatcher AnyOf(params Type[] types)
        {
            var matcher = new Matcher();
            matcher._anyOfTypes = DistinctTypes(types);
            return matcher;
        }

        public static IAnyOfMatcher AnyOf(params IMatcher[] matchers)
        {
            var anyOfMatcher = (Matcher)AnyOf(MergeTypes(matchers));
//            setComponentNames(anyOfMatcher, matchers);
            return anyOfMatcher;
        }

    }
}