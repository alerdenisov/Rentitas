using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Rentitas
{
    public class Scenario : IInitializeSystem, IExecuteSystem, IDeinitializeSystem, ICleanupSystem
    {
        protected readonly List<IInitializeSystem> InitializeSystems;
        protected readonly List<IExecuteSystem> ExecuteSystems;
        protected readonly List<IDeinitializeSystem> DeinitializeSystems;
        protected readonly List<ICleanupSystem> CleanupSystems;

        public Scenario()
        {
            InitializeSystems = new List<IInitializeSystem>();
            ExecuteSystems = new List<IExecuteSystem>();
            DeinitializeSystems = new List<IDeinitializeSystem>();
            CleanupSystems = new List<ICleanupSystem>();
        }

        /// Adds the system instance to the systems list.
        public virtual Scenario Add(ISystem system)
        {
            var reactiveSystem = system as IReactiveIntermalSystem;

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

            return this;
        }

        public virtual void Initialize()
        {
            for (int i = 0; i < InitializeSystems.Count; i++)
            {
                InitializeSystems[i].Initialize();
            }
        }

        public void Execute()
        {
            for (int i = 0; i < ExecuteSystems.Count; i++)
                ExecuteSystems[i].Execute();
        }

        public void Deinitialize()
        {
            for (int i = 0; i < DeinitializeSystems.Count; i++)
                DeinitializeSystems[i].Deinitialize();
        }

        /// Calls Cleanup() on all ICleanupSystem, ReactiveSystem and other nested Systems instances in the order you added them.
        public virtual void Cleanup()
        {
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
                (system as IReactiveIntermalSystem)?.Activate();
                (system as Scenario)?.ActivateReactiveSystems();
            }
        }

        /// Deactivates all ReactiveSystems in the systems list. This will also clear all ReactiveSystems.
        /// This is useful when you want to soft-restart your application and want to reuse your existing system instances.
        public virtual void DeactivateReactiveSystems()
        {
            for (int i = 0; i < ExecuteSystems.Count; i++)
            {
                var system = ExecuteSystems[i];
                (system as IReactiveIntermalSystem)?.Deactivate();
                (system as Scenario)?.DeactivateReactiveSystems();
            }
        }

        /// Clears all ReactiveSystems in the systems list.
        public virtual void ClearReactiveSystems()
        {
            for (int i = 0; i < ExecuteSystems.Count; i++)
            {
                var system = ExecuteSystems[i];
                (system as IReactiveIntermalSystem)?.Clear();
                (system as Scenario)?.ClearReactiveSystems();
            }
        }
    }
}