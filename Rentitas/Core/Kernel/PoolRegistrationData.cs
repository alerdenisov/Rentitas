using System;
using UnityTypeReference;

namespace Rentitas.Kernel
{
    public class PoolRegistrationData
    {
        public TypeRef PoolType;
        public string CustomPoolName;

        public PoolRegistrationData(TypeRef poolType, string customPoolName = null)
        {
            PoolType = poolType;
            CustomPoolName = customPoolName;
        }

        public bool IsCustom => string.IsNullOrEmpty(CustomPoolName);
    }
}