using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Rentitas.Unity
{
    public class ExplorerConfigWidget : IKernelExplorerWidget
    {
        public ExplorerConfigWidget()
        {

        }

        public void DrawWidget(KernelExplorer explorer)
        {
            GUILayout.Label("Initial configuration menu", EditorStyles.boldLabel);
            InjectAll(explorer);
        }

        private void InjectAll(KernelExplorer explorer)
        {

            explorer.Executor.IsNotConfigurated = true;
            var assembly = Assembly.GetAssembly(typeof(IComponent));//E.GetType());
            var poolTypes = assembly.GetTypes().Where(t => typeof(IComponent).IsAssignableFrom(t) && t.IsInterface && t != typeof(IComponent)).ToArray();
            var componentTypes = poolTypes.ToDictionary(
                t => t,
                pool =>
                {
                    return assembly.GetTypes()
                        .Where(t => pool.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface && t != pool).ToArray();
                });


            explorer.Executor.Configs = new RentitasExecutor.PoolInstallConfig[poolTypes.Length];

            for (int i = 0; i < poolTypes.Length; i++)
            {
                var pt = poolTypes[i];
                var ct = componentTypes[pt];
                explorer.Executor.Configs[i] = new RentitasExecutor.PoolInstallConfig(pt, ct);
            }

            explorer.Executor.IsNotConfigurated = true;
        }
    }
}