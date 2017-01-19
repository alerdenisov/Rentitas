using System.Collections.Generic;

namespace Rentitas
{
    public interface IReactiveInternalSystem
    {
        ISystem InternalSubsystem { get; }
        void Activate();
        void Deactivate();
        void Clear();
    }
    /// A ReactiveSystem manages your implementation of a IReactiveSystem, IMultiReactiveSystem or IGroupObserverSystem subsystem.
    /// It will only call subsystem.Execute() if there were changes based on the Triggers and eventTypes specified by your subsystem
    /// and will only pass in changed entities. A common use-case is to react to changes,
    /// e.g. a change of the position of an entity to update the gameObject.transform.position of the related gameObject.
    /// Recommended way to create systems in general: pool.CreateSystem(new MySystem());
    /// This will automatically wrap MySystem in a ReactiveSystem if it implements IReactiveSystem, IMultiReactiveSystem or IGroupObserverSystem.
    public class ReactiveSystem<T> : IReactiveInternalSystem, IExecuteSystem where T : class, IComponent
    {

        /// Returns the subsystem which will be managed by this instance of ReactiveSystem.
        public IReactiveExecuteSystem<T> Subsystem => _subsystem;
        public ISystem InternalSubsystem => Subsystem;

        private readonly IReactiveExecuteSystem<T>      _subsystem;
        private readonly GroupObserver<T>               _observer;
        private readonly IMatcher                       _ensureComponents;
        private readonly IMatcher                       _excludeComponents;
        private readonly bool                           _clearAfterExecute;
        private readonly List<Entity<T>>                _buffer;
        private string                                  _toStringCache;
        private bool                                    _isExecutable;

        /// Recommended way to create systems in general: pool.CreateSystem(new MySystem());
        public ReactiveSystem(Pool<T> pool, IReactiveSystem<T> subSystem) :
            this(subSystem, CreateGroupObserver(pool, new[] { subSystem.Trigger }))
        {
        }

        /// Recommended way to create systems in general: pool.CreateSystem(new MySystem());
        public ReactiveSystem(Pool<T> pool, IMultiReactiveSystem<T> subSystem) :
            this(subSystem, CreateGroupObserver(pool, subSystem.Triggers))
        {
        }

        /// Recommended way to create systems in general: pool.CreateSystem(new MySystem());
        public ReactiveSystem(IGroupObserverSystem<T> subSystem) :
            this(subSystem, subSystem.GroupObserver)
        {
        }

        ReactiveSystem(IReactiveExecuteSystem<T> subSystem, GroupObserver<T> groupObserver)
        {
            _subsystem = subSystem;
            var ensureComponents = subSystem as IEnsureComponents;
            if (ensureComponents != null)
            {
                _ensureComponents = ensureComponents.EnsureComponents;
            }
            var excludeComponents = subSystem as IExcludeComponents;
            if (excludeComponents != null)
            {
                _excludeComponents = excludeComponents.ExcludeComponents;
            }

            _clearAfterExecute = (subSystem as IClearReactiveSystem) != null;

            _observer = groupObserver;
            _buffer = new List<Entity<T>>();

            _isExecutable = subSystem is IExecuteSystem;
        }

        static GroupObserver<T> CreateGroupObserver(Pool<T> pool, TriggerOnEvent[] triggers)
        {
            var triggersLength = triggers.Length;
            var groups = new Group<T>[triggersLength];
            var eventTypes = new GroupEventType[triggersLength];
            for (int i = 0; i < triggersLength; i++)
            {
                var trigger = triggers[i];
                groups[i] = pool.GetGroup(trigger.Trigger);
                eventTypes[i] = trigger.EventType;
            }

            return new GroupObserver<T>(groups, eventTypes);
        }

        /// Activates the ReactiveSystem (ReactiveSystem are activated by default) and starts observing changes
        /// based on the Triggers and eventTypes specified by the subsystem.
        public void Activate()
        {
            _observer.Activate();
        }

        /// Deactivates the ReactiveSystem (ReactiveSystem are activated by default).
        /// No changes will be tracked while deactivated.
        /// This will also clear the ReactiveSystems.
        public void Deactivate()
        {
            _observer.Deactivate();
        }

        /// Clears all accumulated changes.
        public void Clear()
        {
            _observer.ClearCollectedEntities();
        }

        /// Will call subsystem.Execute() with changed entities if there are any. Otherwise it will not call subsystem.Execute().
        public void Execute()
        {
            if (_observer.collectedEntities.Count != 0)
            {
                if (_ensureComponents != null)
                {
                    if (_excludeComponents != null)
                    {
                        foreach (var e in _observer.collectedEntities)
                        {
                            if (_ensureComponents.Matches(e) && !_excludeComponents.Matches(e))
                            {
                                _buffer.Add(e.Retain(this));
                            }
                        }
                    }
                    else
                    {
                        foreach (var e in _observer.collectedEntities)
                        {
                            if (_ensureComponents.Matches(e))
                            {
                                _buffer.Add(e.Retain(this));
                            }
                        }
                    }
                }
                else if (_excludeComponents != null)
                {
                    foreach (var e in _observer.collectedEntities)
                    {
                        if (!_excludeComponents.Matches(e))
                        {
                            _buffer.Add(e.Retain(this));
                        }
                    }
                }
                else
                {
                    foreach (var e in _observer.collectedEntities)
                    {
                        _buffer.Add(e.Retain(this));
                    }
                }

                _observer.ClearCollectedEntities();
                if (_buffer.Count != 0)
                {
                    _subsystem.Execute(_buffer);
                    for (int i = 0; i < _buffer.Count; i++)
                    {
                        _buffer[i].Release(this);
                    }
                    _buffer.Clear();
                    if (_clearAfterExecute)
                    {
                        _observer.ClearCollectedEntities();
                    }
                }
            }

            if(_isExecutable) ((IExecuteSystem)Subsystem).Execute();
        }

        public override string ToString()
        {
            if (_toStringCache == null)
            {
                _toStringCache = "ReactiveSystem(" + Subsystem + ")";
            }

            return _toStringCache;
        }

        ~ReactiveSystem()
        {
            Deactivate();
        }
    }
}