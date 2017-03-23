using System.Collections.Generic;

namespace Rentitas
{
    public class BaseScenario : IInitializeSystem, IExecuteSystem, IDeinitializeSystem, ICleanupSystem
    {
        private bool enabled;

        protected readonly List<ISetApplication> SetupApplicationSystems;
        protected readonly List<ISetPools> SetupPoolsSystems;
        protected readonly List<IInitializeSystem> InitializeSystems;
        protected readonly List<IExecuteSystem> ExecuteSystems;
        protected readonly List<IDeinitializeSystem> DeinitializeSystems;
        protected readonly List<ICleanupSystem> CleanupSystems;
        protected readonly List<IEnableSystem> EnableSystems;
        protected readonly List<IDisableSystem> DisableSystems;

        public BaseScenario(string name = "")
        {
            SetupApplicationSystems = new List<ISetApplication>();
            SetupPoolsSystems = new List<ISetPools>();
            InitializeSystems = new List<IInitializeSystem>();
            ExecuteSystems = new List<IExecuteSystem>();
            DeinitializeSystems = new List<IDeinitializeSystem>();
            CleanupSystems = new List<ICleanupSystem>();
            EnableSystems = new List<IEnableSystem>();
            DisableSystems = new List<IDisableSystem>();

            enabled = true;
        }

        /// Adds the system instance to the systems list.
        public virtual BaseScenario Add(ISystem system)
        {
            var reactiveSystem = system as IReactiveInternalSystem;

            var setupApplicationSystem = reactiveSystem != null
                ? reactiveSystem.InternalSubsystem as ISetApplication
                : system as ISetApplication;

            if (setupApplicationSystem != null)
            {
                SetupApplicationSystems.Add(setupApplicationSystem);
            }
            
            var setupPoolsSystem = reactiveSystem != null
                ? reactiveSystem.InternalSubsystem as ISetPools
                : system as ISetPools;

            if (setupPoolsSystem != null)
            {
                SetupPoolsSystems.Add(setupPoolsSystem);
            }

            var initializeSystem = reactiveSystem != null
                ? reactiveSystem.InternalSubsystem as IInitializeSystem
                : system as IInitializeSystem;

            if (initializeSystem != null)
            {
                InitializeSystems.Add(initializeSystem);
            }

            var executeSystem = system as IExecuteSystem;
            if (executeSystem != null)
            {
                ExecuteSystems.Add(executeSystem);
            }

            var cleanupSystem = reactiveSystem != null
                ? reactiveSystem.InternalSubsystem as ICleanupSystem
                : system as ICleanupSystem;

            if (cleanupSystem != null)
            {
                CleanupSystems.Add(cleanupSystem);
            }

            var deinitializeSystem = reactiveSystem != null
                ? reactiveSystem.InternalSubsystem as IDeinitializeSystem
                : system as IDeinitializeSystem;

            if (deinitializeSystem != null)
            {
                DeinitializeSystems.Add(deinitializeSystem);
            }

            var enableSystem = reactiveSystem != null
                ? reactiveSystem.InternalSubsystem as IEnableSystem
                : system as IEnableSystem;

            if (enableSystem != null)
            {
                EnableSystems.Add(enableSystem);
            }

            var disableSystem = reactiveSystem != null
                ? reactiveSystem.InternalSubsystem as IDisableSystem
                : system as IDisableSystem;

            if (disableSystem != null)
            {
                DisableSystems.Add(disableSystem);
            }

            return this;
        }

        public virtual void SetPools(Pools pools)
        {
            for (int i = 0; i < SetupPoolsSystems.Count; i++)
            {
                SetupPoolsSystems[i].SetPools(pools);
                SetupPoolsSystems.RemoveAt(i);
                i--;
            }
        }

        public virtual void SetApplication(IApplication app)
        {
            for (int i = 0; i < SetupApplicationSystems.Count; i++)
            {
                SetupApplicationSystems[i].SetApplication(app);
                SetupApplicationSystems.RemoveAt(i);
                i--;
            }
        }

        public virtual void Initialize()
        {
            for (int i = 0; i < InitializeSystems.Count; i++)
            {
                InitializeSystems[i].Initialize();
            }
        }

        public virtual void Execute()
        {
            if (!enabled)
            {
                return;
            }

            for (int i = 0; i < ExecuteSystems.Count; i++)
                ExecuteSystems[i].Execute();
        }

        public virtual void Deinitialize()
        {
            for (int i = 0; i < DeinitializeSystems.Count; i++)
                DeinitializeSystems[i].Deinitialize();
        }

        /// Calls Cleanup() on all ICleanupSystem, ReactiveSystem and other nested Systems instances in the order you added them.
        public virtual void Cleanup()
        {
            if (!enabled)
            {
                return;
            }

            for (int i = 0; i < CleanupSystems.Count; i++)
            {
                CleanupSystems[i].Cleanup();
            }
        }

        /// Activates all ReactiveSystems in the systems list.
        public virtual void ActivateReactiveSystems()
        {
            for (int i = 0; i < ExecuteSystems.Count; i++)
            {
                var system = ExecuteSystems[i];
                (system as IReactiveInternalSystem)?.Activate();
                (system as BaseScenario)?.ActivateReactiveSystems();
            }
        }

        /// Deactivates all ReactiveSystems in the systems list. This will also clear all ReactiveSystems.
        /// This is useful when you want to soft-restart your application and want to reuse your existing system instances.
        public virtual void DeactivateReactiveSystems()
        {
            for (int i = 0; i < ExecuteSystems.Count; i++)
            {
                var system = ExecuteSystems[i];
                (system as IReactiveInternalSystem)?.Deactivate();
                (system as BaseScenario)?.DeactivateReactiveSystems();
            }
        }

        /// Clears all ReactiveSystems in the systems list.
        public virtual void ClearReactiveSystems()
        {
            for (int i = 0; i < ExecuteSystems.Count; i++)
            {
                var system = ExecuteSystems[i];
                (system as IReactiveInternalSystem)?.Clear();
                (system as BaseScenario)?.ClearReactiveSystems();
            }
        }

        public virtual void Remove(BaseScenario scenario)
        {
            scenario.ClearReactiveSystems();
            scenario.Cleanup();
            scenario.Deinitialize();
            InitializeSystems.Remove(scenario as IInitializeSystem);
            ExecuteSystems.Remove(scenario as IExecuteSystem);
            DeinitializeSystems.Remove(scenario as IDeinitializeSystem);
            CleanupSystems.Remove(scenario as ICleanupSystem);
        }

        public void Disable()
        {
            foreach (var disableSystem in DisableSystems)
            {
                disableSystem.Disable();
            }

            enabled = false;
            DeactivateReactiveSystems();
        }

        public void Enable()
        {
            foreach (var enableSystem in EnableSystems)
            {
                enableSystem.Enable();
            }
            
            enabled = true;
            ActivateReactiveSystems();
        }
    }
}