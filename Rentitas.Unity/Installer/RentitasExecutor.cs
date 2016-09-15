using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTypeReference;

namespace Rentitas.Unity
{
    public class RentitasExecutor : MonoBehaviour
    {
        [Serializable]
        public class PoolInstallComponentConfig
        {
            public TypeRef ComponentType;
            public bool IsActive;

            public PoolInstallComponentConfig(Type componentType)
            {
                ComponentType = componentType;
            }
        }

        [Serializable]
        public class PoolInstallConfig
        {
            public bool IsActive;

            [ClassImplements(typeof(IComponent))]
            public TypeRef PoolType;

            public PoolInstallComponentConfig[] ComponentTypes;

            public PoolInstallConfig(Type pool, Type[] components)
            {
                PoolType = pool;
                if (components.Length > 0)
                {
                    ComponentTypes = new PoolInstallComponentConfig[components.Length];

                    for (int i = 0; i < components.Length; i++)
                        ComponentTypes[i] = new PoolInstallComponentConfig(components[i]);
                }
            }
        }

        public PoolInstallConfig[] Configs;
        public bool IsNotConfigurated;

        public PoolInstallConfig GetConfig<T>() where T : class, IComponent
        { 
            return GetConfig(typeof(T));
        }

        public PoolInstallConfig GetConfig(Type pool)
        {
            return Configs.FirstOrDefault(c => c.PoolType.Type == pool);
        }
    }
}