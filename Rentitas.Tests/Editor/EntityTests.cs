using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Rentitas.Tests
{
    [TestFixture]
    public class EntityContext
    {
        protected TestComponentA refA;
        protected TestComponentB refB;
        protected TestComponentC refC;
        protected Entity<ITestPool> entity;
        protected Pool<ITestPool> pool;

        [SetUp]
        public void Setup()
        {
            refA = new TestComponentA();
            refB = new TestComponentB();
            refC = new TestComponentC();

            pool = new Pool<ITestPool>(refA, refB, refC);

            entity = pool.CreateEntity();
        }

        protected void AssertHasComponentA(Entity<ITestPool> e, TestComponentA comp = null)
        {
            if (comp == null)
                comp = refA;
            var ea = e.Get<TestComponentA>();
            Assert.AreSame(ea, comp);

            var components = e.GetComponents();
            Assert.AreEqual(1, components.Length);
            Assert.Contains(comp, components);

            var types = e.GetComponentsTypes();
            Assert.AreEqual(1, types.Length);
            Assert.Contains(typeof(TestComponentA), types);

            Assert.IsTrue(e.Has<TestComponentA>());
            Assert.IsTrue(e.Has(typeof(TestComponentA)));
            Assert.IsTrue(e.HasAny(typeof(TestComponentA)));
        }

        protected void AssertHasNotComponentA(Entity<ITestPool> e)
        {
            var components = e.GetComponents();
            Assert.AreEqual(0, components.Length);

            var types = e.GetComponentsTypes();
            Assert.AreEqual(0, types.Length);

            Assert.IsFalse(e.Has<TestComponentA>());
            Assert.IsFalse(e.Has(typeof(TestComponentA)));
            Assert.IsFalse(e.HasAny(typeof(TestComponentA)));
        }
    }

    [TestFixture]
    public class EventDispatchContext : EntityContext
    {
        protected int did;

        [SetUp]
        public void Setup1()
        {
            did = 0;
        }
    }

    [TestFixture]
    public class AfterComponentAddedContext : EntityContext
    {
        [SetUp]
        public void Setup2()
        {
            entity.Add<TestComponentA>();
        }

    }

    [TestFixture]
    public class EntityCacheContext : AfterComponentAddedContext
    {
        protected IComponent[] cache;
        protected Type[] types;

        [SetUp]
        public void Setup3()
        {
            cache = entity.GetComponents();
            types = entity.GetComponentsTypes();
        }
    }


    [TestFixture]
    public class WhenAddingAnotherComponentContext : AfterComponentAddedContext
    {
        [SetUp]
        public void Setup3()
        {
            entity.Add<TestComponentB>();
        }
    }


    public class EntityTests : EntityContext
    {
        [Test]
        public void has_defaul_poolmetadata()
        {
            Assert.AreEqual(3, entity.PoolMeta.ComponentNames.Count);
            Assert.AreSame(pool.Meta.ComponentTypes, entity.PoolMeta.ComponentTypes);
            Assert.AreSame(pool.Meta.ComponentNames, entity.PoolMeta.ComponentNames);
        }

        [Test]
        public void throws_when_attempting_to_Get_component_at_type_which_hasnt_beed_added()
        {
            Assert.Catch<EntityDoesNotHaveComponentException<ITestPool>>(() => entity.Get<TestComponentA>());
        }

        [Test]
        public void gets_total_components_count_when_empty()
        {
            Assert.AreEqual(3, entity.TotalComponents);
        }

        [Test]
        public void gets_empty_array_when_empty()
        {
            Assert.IsEmpty(entity.GetComponents());
            Assert.IsEmpty(entity.GetComponentsTypes());
        }

        [Test]
        public void desnt_have_component()
        {
            Assert.IsFalse(entity.Has<TestComponentA>());
        }

        [Test]
        public void desn_hav_any_components()
        {
            Assert.IsFalse(entity.HasAny(typeof (TestComponentA)));
        }

        [Test]
        public void returns_same_entity_after_add_component()
        {
            Assert.AreSame(entity, entity.Add<TestComponentA>());
        }

        [Test]
        public void add_test_component_A()
        {
            entity.Add<TestComponentA>();
            AssertHasComponentA(entity);
        }

        [Test]
        public void throws_when_attempting_to_remove_comp_which_hasnt_been_added()
        {
            Assert.Catch<EntityDoesNotHaveComponentException<ITestPool>>(() => entity.Remove<TestComponentA>());
        }

        [Test]
        public void replacing_non_existing_compoent_adds_it()
        {
            Assert.IsFalse(entity.Has<TestComponentA>());
            entity.Replace<TestComponentA>();
            AssertHasComponentA(entity);
        }

    }

    public class EntityAfterComponentAdded : AfterComponentAddedContext
    {
        [Test]
        public void should_have_component_a()
        {
            AssertHasComponentA(entity);
        }

        [Test]
        public void throw_when_adding_already_exist_component()
        {
            Assert.Catch<EntityAlreadyHasComponentException<ITestPool>>(() => entity.Add<TestComponentA>());
        }

        [Test]
        public void returns_entity_when_removing_component()
        {
            Assert.AreSame(entity, entity.Remove<TestComponentA>());
        }

        [Test]
        public void removes_component()
        {
            entity.Remove<TestComponentA>();
            AssertHasNotComponentA(entity);
        }

        [Test]
        public void return_entity_when_replacing_component()
        {
            Assert.AreSame(entity, entity.Replace<TestComponentA>());
        }

        [Test]
        public void replacing_existing_component_return_new_one()
        {
            AssertHasComponentA(entity);
            entity.Replace<TestComponentA>();
            var newOne = entity.Get<TestComponentA>();
            AssertHasComponentA(entity, newOne);
            Assert.AreNotSame(refA, newOne);
        }

        [Test]
        public void doesnt_have_components_at_type_when_not_all_components_added()
        {
            Assert.IsFalse(entity.Has(typeof (TestComponentA), typeof (TestComponentB)));
        }

        [Test]
        public void has_any_components_at_indices_when_any_component_added()
        {
            Assert.IsTrue(entity.HasAny(typeof (TestComponentA), typeof (TestComponentB)));
        }


        [Test]
        public void pushes_component_to_pool_when_removed()
        {
            var pool = entity.GetComponentPool(typeof (TestComponentA));
            Assert.AreEqual(0, pool.Count);

            entity.Remove<TestComponentA>();

            Assert.AreEqual(1, pool.Count);
        }

        [Test]
        public void creates_new_component_when_pool_is_empty()
        {
            var newOne = entity.CreateComponent<TestComponentA>();
            Assert.AreNotSame(newOne, refA);
        }

        [Test]
        public void gets_pooled_component()
        {
            var compA = entity.Get<TestComponentA>();
            var pool = entity.GetComponentPool(typeof (TestComponentA));

            Assert.AreEqual(0, pool.Count);
            entity.Remove<TestComponentA>();

            Assert.AreEqual(1, pool.Count);

            entity.Add<TestComponentA>();

            Assert.AreEqual(0, pool.Count);
            Assert.AreSame(compA, entity.Get<TestComponentA>());
        }
    }

    public class EntityWhenAddingAnotherComponentTest : WhenAddingAnotherComponentContext
    {
        [Test]
        public void gets_all_components()
        {
            var components = entity.GetComponents();
            Assert.AreEqual(2, components.Length);
            Assert.Contains(refA, components);
            Assert.Contains(refB, components);
        }

        [Test]
        public void gets_all_component_types()
        {
            var types = entity.GetComponentsTypes();
            Assert.AreEqual(2, types.Length);
            Assert.Contains(typeof(TestComponentA), types);
            Assert.Contains(typeof(TestComponentB), types);
        }

        [Test]
        public void has_another_component()
        {
            Assert.IsTrue(entity.Has<TestComponentB>());
        }

        [Test]
        public void can_toString()
        {
            entity.Retain(this);
            Assert.AreEqual("[0] Entity [Owners: 2] [Components: 2] TestComponentA, TestComponentB", entity.ToString());
        }
    }

    public class EntityEventsTests: EventDispatchContext
    {
        [Test]
        public void dispatches_OnComponentAdded_when_adding_a_component()
        {
            entity.OnComponentAdded += (e, type, component) =>
            {
                did += 1;
                Assert.AreSame(e, entity);
                Assert.AreEqual(typeof(TestComponentA), type);
                Assert.AreSame(refA, component);
            };

            entity.OnComponentRemoved += delegate { Assert.Fail(); };
            entity.OnComponentReplaced += delegate { Assert.Fail(); };

            entity.Add<TestComponentA>();

            Assert.AreEqual(1, did);
        }

        [Test]
        public void dispatches_OnComponentRemove_when_removing_component()
        {
            entity.Add<TestComponentA>();

            entity.OnComponentRemoved += (e, type, component) =>
            {
                did += 1;
                Assert.AreSame(e, entity);
                Assert.AreEqual(typeof(TestComponentA), type);
                Assert.AreSame(refA, component);
            };


            entity.OnComponentAdded += delegate { Assert.Fail(); };
            entity.OnComponentReplaced += delegate { Assert.Fail(); };

            entity.Remove<TestComponentA>();

            Assert.AreEqual(1, did);
        }

        [Test]
        public void dispatches_OnComponentRemoved_before_pushing_component_to_pool()
        {
            entity.Add<TestComponentA>();

            entity.OnComponentRemoved += (e, type, component) =>
            {
                var newcomponent = entity.CreateComponent<TestComponentA>();
                Assert.AreNotSame(component, newcomponent);
            };

            entity.Remove<TestComponentA>();
        }

        [Test]
        public void dispatches_OnComponentReplaced_when_replacing_a_component()
        {
            entity.Add<TestComponentA>();

            var newA = new TestComponentA();

            entity.OnComponentReplaced += (e, type, prev, next) =>
            {
                did += 1;
                Assert.AreSame(e, entity);
                Assert.AreEqual(typeof(TestComponentA), type);
                Assert.AreSame(refA, prev);
                Assert.AreNotSame(refA, next);
                Assert.AreNotSame(refA, next);
            };

            entity.OnComponentAdded     += delegate { Assert.Fail(); };
            entity.OnComponentRemoved   += delegate { Assert.Fail(); };


            entity.ReplaceInstance(newA);

            Assert.AreEqual(did, 1);
        }

        [Test]
        public void provides_previous_and_new_component_OnComponentReplaced_when_replacing_with_different_component()
        {
            var prevA = entity.CreateComponent<TestComponentA>();
            var nextA = entity.CreateComponent<TestComponentA>();
            Assert.AreSame(prevA, refA);
            Assert.AreNotSame(prevA, nextA);

            entity.OnComponentReplaced += (e, type, prev, next) =>
            {
                did ++;
                Assert.AreSame(e, entity);
                Assert.AreSame(prev, prevA);
                Assert.AreSame(next, nextA);
                Assert.AreNotSame(prev, next);
            };

            entity.AddInstance(prevA);
            entity.ReplaceInstance(nextA);
            Assert.AreEqual(1, did);
        }

        [Test]
        public void provides_prev_and_next_component_OnComponentReplaced_when_replacing_with_same()
        {
            entity.OnComponentReplaced += (e, type, prev, next) =>
            {
                did++;
                Assert.AreSame(e, entity);
                Assert.AreSame(prev, next);
                Assert.AreSame(prev, refA);
                Assert.AreSame(next, refA);

            };

            entity.Add<TestComponentA>();
            entity.ReplaceInstance(refA);
            Assert.AreEqual(1, did);
        }

        [Test]
        public void doesnt_dispatch_any_when_replacing_non_existing_with_null()
        {
            entity.OnComponentAdded += delegate { Assert.Fail(); };
            entity.OnComponentRemoved += delegate { Assert.Fail(); };
            entity.OnComponentReplaced += delegate { Assert.Fail(); };

            entity.ReplaceInstance<TestComponentA>(null);
        }

        [Test]
        public void dispatches_OnComponentAdded_when_Attempting_to_replace_component_whitch_doesnt_added()
        {
            entity.OnComponentAdded += (e, type, component) =>
            {
                did += 1;
                Assert.AreSame(e, entity);
                Assert.AreEqual(typeof(TestComponentA), type);
                Assert.AreSame(refA, component);
            };

            entity.OnComponentRemoved += delegate { Assert.Fail(); };
            entity.OnComponentReplaced += delegate { Assert.Fail(); };

            entity.Replace<TestComponentA>();

            Assert.AreEqual(1, did);

        }

        [Test]
        public void dispatched_OnComponentRemoved_when_replacing_component_with_null()
        {
            entity.Add<TestComponentA>();
            entity.OnComponentRemoved += (e, type, component) =>
            {
                did += 1;
                Assert.AreSame(component, refA);
            };
            entity.OnComponentAdded += delegate { Assert.Fail(); };
            entity.OnComponentReplaced += delegate { Assert.Fail(); };

            entity.ReplaceInstance<TestComponentA>(null);
            Assert.AreEqual(did, 1);
        }

        [Test]
        public void dispatches_OnCompoentRemoved_when_removing_all_components()
        {

            entity.OnComponentRemoved += delegate { did++; };
            entity.Add<TestComponentA>().Add<TestComponentB>().RemoveAllComponents();

            Assert.AreEqual(2, did);
        }

        [Test]
        public void doesnt_dispatch_OnEntityReleased_when_retaining()
        {
            entity.OnEntityReleased += delegate { Assert.Fail(); };
            entity.Retain(this);
        }
        
    }

    public class EntityReferencesCountingTests : EntityContext
    {
        [Test]
        public void retains_entity()
        {
            Assert.AreEqual(1, entity.RetainCount);
            entity.Retain(this);
            Assert.AreEqual(2, entity.RetainCount);
            Assert.Contains(this, entity.owners.ToArray());
        }

        [Test]
        public void releases_entity()
        {
            Assert.AreEqual(1, entity.RetainCount);
            entity.Retain(this);
            entity.Release(this);
            Assert.AreEqual(1, entity.RetainCount);
        }

        [Test]
        public void throws_when_releasing_not_owner_object()
        {
            Assert.Catch<EntityIsNotRetainedByOwnerException<ITestPool>>(() => entity.Release(this));
        }

        [Test]
        public void throws_when_retaining_twice()
        {
            entity.Retain(this);
            Assert.Catch<EntityIsAlreadyRetainedByOwnerException<ITestPool>>(() => entity.Retain(this));
        }
    }

    public class EntityInternalCaching : EntityCacheContext
    {
        [Test]
        public void updates_cache_when_new_component_added()
        {
            entity.Add<TestComponentB>();
            Assert.AreNotSame(cache, entity.GetComponents());
        }

        [Test]
        public void updates_cache_when_component_removed()
        {
            entity.Remove<TestComponentA>();
            Assert.AreNotSame(cache, entity.GetComponents());
        }

        [Test]
        public void updates_cache_when_component_replaced()
        {
            entity.Replace<TestComponentA>();
            Assert.AreNotSame(cache, entity.GetComponents());
        }

        [Test]
        public void doesnt_update_cache_when_component_was_replaced_with_same()
        {
            Assert.AreSame(refA, entity.Get<TestComponentA>());
            entity.ReplaceInstance(refA);

            Assert.AreSame(cache, entity.GetComponents());
        }

        [Test]
        public void updates_cache_when_all_component_was_removed()
        {
            entity.RemoveAllComponents();
            Assert.AreNotSame(cache, entity.GetComponents());
        }

        [Test]
        public void caches_component_types()
        {
            Assert.AreSame(types, entity.GetComponentsTypes());
        }

        [Test]
        public void updates_types_when_new_component_was_added()
        {
            entity.Add<TestComponentB>();
            Assert.AreNotSame(types, entity.GetComponentsTypes());
        }

        [Test]
        public void updates_types_when_component_removed()
        {
            entity.Remove<TestComponentA>();
            Assert.AreNotSame(types, entity.GetComponentsTypes());
        }

        [Test]
        public void updates_cache_when_adding_component_with_Replace()
        {
            entity.Replace<TestComponentC>();
            Assert.AreNotSame(types, entity.GetComponentsTypes());
        }

        [Test]
        public void updates_cached_when_all_components_removed()
        {
            entity.RemoveAllComponents();
            Assert.AreNotSame(types, entity.GetComponentsTypes());
        }

        [Test]
        public void doesnt_update_types_when_component_replaced()
        {
            entity.Replace<TestComponentA>();
            Assert.AreSame(types, entity.GetComponentsTypes());
        }
    }
}