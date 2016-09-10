using System;

namespace Rentitas
{
    public partial class Matcher
    {

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType() || obj.GetHashCode() != GetHashCode())
            {
                return false;
            }

            var matcher = (Matcher)obj;
            if (!EqualTypes(matcher.AllOfTypes, _allOfTypes))
            {
                return false;
            }
            if (!EqualTypes(matcher.AnyOfTypes, _anyOfTypes))
            {
                return false;
            }
            if (!EqualTypes(matcher.NoneOfTypes, _noneOfTypes))
            {
                return false;
            }

            return true;
        }

        static bool EqualTypes(Type[] i1, Type[] i2)
        {
            if ((i1 == null) != (i2 == null))
            {
                return false;
            }
            if (i1 == null)
            {
                return true;
            }
            if (i1.Length != i2.Length)
            {
                return false;
            }

            for (int i = 0; i < i1.Length; i++)
            {
                if (i1[i] != i2[i])
                {
                    return false;
                }
            }

            return true;
        }

        int _hash;
        bool _isHashCached;

        public override int GetHashCode()
        {
            if (!_isHashCached)
            {
                var hash = GetType().GetHashCode();
                hash = ApplyHash(hash, _allOfTypes, 3, 53);
                hash = ApplyHash(hash, _anyOfTypes, 307, 367);
                hash = ApplyHash(hash, _noneOfTypes, 647, 683);
                _hash = hash;
                _isHashCached = true;
            }

            return _hash;
        }

        static int ApplyHash(int hash, Type[] indices, int i1, int i2)
        {
            if (indices != null)
            {
                for (int i = 0; i < indices.Length; i++)
                {
                    hash ^= indices[i].GetHashCode() * i1;
                }
                hash ^= indices.Length * i2;
            }

            return hash;
        }
    }
}