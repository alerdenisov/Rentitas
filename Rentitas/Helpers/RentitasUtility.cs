using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rentitas
{
    public class ComponentsCollection<TPool>
            where TPool : IComponent
    {
        private HashSet<TPool> _list;

        public TPool[] List { get { return _list.ToArray(); } }

        public ComponentsCollection(IEnumerable<TPool> initialCollection)
        {
            _list = new HashSet<TPool>(initialCollection);
        }

        public ComponentsCollection<TPool> Add(TPool component)
        {
            if(!_list.Add(component))
                throw new ArgumentException($"Component {component} already in collection", nameof(component));

            return this;
        }

        public TPool[] Build()
        {
            return _list.Where(i => i != null).ToArray();
        }

        public TComponent[] Build<TComponent>() where TComponent : IComponent
        {
            if (!typeof(TComponent).IsAssignableFrom(typeof(TPool)))
            {
                throw new ArgumentException("TComponent type isn't assignable from TPool");
            }

            return _list.Where(i => i != null).OfType<TComponent>().ToArray();
        }
    }
    public static class RentitasUtility
    {
        public static ComponentsCollection<TPool> CollectComponents<TPool>()
            where TPool : IComponent
        {
            return CollectComponents<TPool>(typeof(TPool).Assembly);
        }

        public static ComponentsCollection<TPool> CollectComponents<TPool>(Assembly where)
            where TPool : IComponent
        {
            return new ComponentsCollection<TPool>(where.GetTypes()
                .Where(t => typeof(TPool).IsAssignableFrom(t) && !t.IsAbstract)
                .Where(t => !t.GetCustomAttributes(true).Any(a => a is ObsoleteAttribute))
                .Where(t => !t.IsGenericType)
                .Select(t => (TPool) Activator.CreateInstance(t)));
        }
    }
}
