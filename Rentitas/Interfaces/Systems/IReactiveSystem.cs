using System.Collections.Generic;

namespace Rentitas
{
    /// Implement this interface if you want to create a reactive system which is triggered by the specified Trigger.
    public interface IReactiveSystem<T> : IReactiveExecuteSystem<T> where T : class, IComponent
    {
        TriggerOnEvent Trigger { get; }
    }

    /// Implement this interface if you want to create a reactive system which is triggered by any of the specified Triggers.
    public interface IMultiReactiveSystem<T> : IReactiveExecuteSystem<T> where T : class, IComponent
    {
        TriggerOnEvent[] Triggers { get; }
    }

    /// Implement this interface if you want to create a reactive system which is triggered by a GroupObserver.
    /// This is useful when you want to react to changes in multiple groups from different pools.
    public interface IGroupObserverSystem<T> : IReactiveExecuteSystem<T> where T : class, IComponent
    {
        GroupObserver<T> GroupObserver { get; }
    }

    /// Not meant to be implemented. Use either IReactiveSystem or IMultiReactiveSystem.
    public interface IReactiveExecuteSystem<T> : ISystem where T : class, IComponent
    {
        void Execute(List<Entity<T>> entities);
    }

    /// Implement this interface in combination with IReactiveSystem or IMultiReactiveSystem.
    /// It will ensure that all entities will match the specified matcher.
    /// This is useful when a component triggered the reactive system, but once the system gets executed the component already has been removed.
    /// Implementing IEnsureComponents can filter these enities.
    public interface IEnsureComponents
    {
        IMatcher EnsureComponents { get; }
    }

    /// Implement this interface in combination with IReactiveSystem or IMultiReactiveSystem.
    /// It will exclude all entities which match the specified matcher.
    /// To exclude multiple components use Matcher.AnyOf(ComponentX, ComponentY, ComponentZ).
    public interface IExcludeComponents
    {
        IMatcher ExcludeComponents { get; }
    }

    /// Implement this interface in combination with IReactiveSystem or IMultiReactiveSystem.
    /// If a system changes entities which in turn would Trigger itself consider implementing IClearReactiveSystem
    /// which will ignore the changes made by the system.
    public interface IClearReactiveSystem
    {
    }
}