using System.Linq;
using Rentitas.Kernel;
using UnityEngine;
using UnityTypeReference;

namespace Rentitas.Unity.Kernels
{
    [CreateAssetMenu(fileName = "New Kernel", menuName = "Kernel")]
    public class KernelObject : ScriptableObject//, IKernel
    {
        [ClassImplements(typeof(IComponent), TypesAllowed.Interface)]
        public TypeRef[] PoolTypes;

        public BaseScenario Scenario { get; }

        public PoolRegistrationData[] PoolInterfaces
        {
            get
            {
                return PoolTypes.Where(pt => pt.Type != null).Select(pt =>
                {
                    var type = pt.Type;
                    return new PoolRegistrationData(type);
                }).ToArray();
            }
        }
    }
}