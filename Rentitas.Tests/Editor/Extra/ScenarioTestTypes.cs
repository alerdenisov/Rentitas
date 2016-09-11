using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rentitas.Tests.Extra
{
    public class CleanupSystemSpy : ICleanupSystem
    {
        public int didCleanup { get { return _didCleanup; } }

        int _didCleanup;

        public void Cleanup()
        {
            _didCleanup += 1;
        }
    }

    public class DeinitializeSystemSpy : IDeinitializeSystem
    {

        public int didDeinitialize { get { return _didDeinitialize; } }

        int _didDeinitialize;

        public void Deinitialize()
        {
            _didDeinitialize += 1;
        }
    }

    public class ExecuteSystemSpy : IExecuteSystem
    {

        public int didExecute { get { return _didExecute; } }

        int _didExecute;

        public void Execute()
        {
            _didExecute += 1;
        }
    }
    public class InitializeSystemSpy : IInitializeSystem
    {

        public int didInitialize { get { return _didInitialize; } }

        int _didInitialize;

        public void Initialize()
        {
            _didInitialize += 1;
        }
    }


    public class MultiReactiveSubSystemSpy : ReactiveSubSystemSpyBase, IMultiReactiveSystem<ITestPool>
    {

        public TriggerOnEvent[] Triggers { get { return _triggers; } }

        readonly TriggerOnEvent[] _triggers;

        public MultiReactiveSubSystemSpy(TriggerOnEvent[] triggers)
        {
            _triggers = triggers;
        }
    }

    public class MultiReactiveEnsureSubSystemSpy : MultiReactiveSubSystemSpy, IEnsureComponents
    {

        public IMatcher EnsureComponents { get { return _ensureComponents; } }

        readonly IMatcher _ensureComponents;

        public MultiReactiveEnsureSubSystemSpy(TriggerOnEvent[] triggers, IMatcher ensureComponents) :
            base(triggers)
        {
            _ensureComponents = ensureComponents;
        }
    }

    public class MultiReactiveExcludeSubSystemSpy : MultiReactiveSubSystemSpy, IExcludeComponents
    {

        public IMatcher ExcludeComponents { get { return _excludeComponents; } }

        readonly IMatcher _excludeComponents;

        public MultiReactiveExcludeSubSystemSpy(TriggerOnEvent[] triggers, IMatcher excludeComponents) :
            base(triggers)
        {
            _excludeComponents = excludeComponents;
        }
    }

    public class MultiReactiveEnsureExcludeSubSystemSpy : MultiReactiveSubSystemSpy, IEnsureComponents, IExcludeComponents
    {

        public IMatcher EnsureComponents { get { return _ensureComponents; } }
        public IMatcher ExcludeComponents { get { return _excludeComponents; } }

        readonly IMatcher _ensureComponents;
        readonly IMatcher _excludeComponents;

        public MultiReactiveEnsureExcludeSubSystemSpy(TriggerOnEvent[] triggers, IMatcher ensureComponents, IMatcher excludeComponents) :
            base(triggers)
        {
            _ensureComponents = ensureComponents;
            _excludeComponents = excludeComponents;
        }
    }

    public class ReactiveSubSystemSpy : ReactiveSubSystemSpyBase, IReactiveSystem<ITestPool>
    {

        public TriggerOnEvent Trigger { get { return new TriggerOnEvent(_matcher, _eventType); } }

        readonly IMatcher _matcher;
        readonly GroupEventType _eventType;

        public ReactiveSubSystemSpy(IMatcher matcher, GroupEventType eventType)
        {
            _matcher = matcher;
            _eventType = eventType;
        }
    }

    public class ClearReactiveSubSystemSpy : ReactiveSubSystemSpy, IClearReactiveSystem
    {
        public ClearReactiveSubSystemSpy(IMatcher matcher, GroupEventType eventType) :
            base(matcher, eventType)
        {
        }
    }

    public class ReactiveEnsureSubSystemSpy : ReactiveSubSystemSpy, IEnsureComponents
    {

        public IMatcher EnsureComponents { get { return _ensureComponent; } }

        readonly IMatcher _ensureComponent;

        public ReactiveEnsureSubSystemSpy(IMatcher matcher, GroupEventType eventType, IMatcher ensureComponent) :
            base(matcher, eventType)
        {
            _ensureComponent = ensureComponent;
        }
    }

    public class ReactiveExcludeSubSystemSpy : ReactiveSubSystemSpy, IExcludeComponents
    {

        public IMatcher ExcludeComponents { get { return _excludeComponent; } }

        readonly IMatcher _excludeComponent;

        public ReactiveExcludeSubSystemSpy(IMatcher matcher, GroupEventType eventType, IMatcher excludeComponent) :
            base(matcher, eventType)
        {
            _excludeComponent = excludeComponent;
        }
    }

    public class ReactiveEnsureExcludeSubSystemSpy : ReactiveSubSystemSpy, IEnsureComponents, IExcludeComponents
    {

        public IMatcher EnsureComponents { get { return _ensureComponent; } }
        public IMatcher ExcludeComponents { get { return _excludeComponent; } }

        readonly IMatcher _ensureComponent;
        readonly IMatcher _excludeComponent;

        public ReactiveEnsureExcludeSubSystemSpy(IMatcher matcher, GroupEventType eventType, IMatcher ensureComponent, IMatcher excludeComponent) :
            base(matcher, eventType)
        {
            _ensureComponent = ensureComponent;
            _excludeComponent = excludeComponent;
        }
    }


    public interface IReactiveSubSystemSpy
    {
        int didInitialize { get; }
        int didExecute { get; }
        int didCleanup { get; }
        int didDeinitialize { get; }
        Entity<ITestPool>[] entities { get; }
    }

    public class InitializeExecuteCleanupDeinitializeSystemSpy : ReactiveSubSystemSpyBase, IExecuteSystem
    {
        public void Execute()
        {
            Execute(null);
        }
    }

    public abstract class ReactiveSubSystemSpyBase : IReactiveSubSystemSpy, IInitializeSystem, ICleanupSystem, IDeinitializeSystem
    {

        public int didInitialize => _didInitialize;
        public int didExecute => _didExecute;
        public int didCleanup => _didCleanup;
        public int didDeinitialize => _didDeinitialize;
        public Entity<ITestPool>[] entities => _entities;

        public Action<List<Entity<ITestPool>>> executeAction;

        protected int _didInitialize;
        protected int _didExecute;
        protected int _didCleanup;
        protected int _didDeinitialize;
        protected Entity<ITestPool>[] _entities;

        public void Initialize()
        {
            _didInitialize += 1;
        }

        public void Execute(List<Entity<ITestPool>> entities)
        {
            _didExecute += 1;

            if (entities != null)
            {
                _entities = entities.ToArray();
            }
            else
            {
                _entities = null;
            }

            executeAction?.Invoke(entities);
        }

        public void Cleanup()
        {
            _didCleanup += 1;
        }

        public void Deinitialize()
        {
            _didDeinitialize += 1;
        }
    }

}