using System;
using UnityEngine;
using NUnit.Framework;

namespace Rentitas.Tests
{
    public interface ITestPool : IComponent { }
    public interface ITestSecondPool : IComponent { }

    public class TestComponentA : ITestPool { }
    public class TestComponentB : ITestPool { }
    public class TestComponentC : ITestPool { }

    public class TestNameComponent : ITestSecondPool
    {
        public string Name;
    }

    public class Pools
    {
        private Pool<ITestPool> TestPool(int creationIndex = 0)
        {
            return new Pool<ITestPool>(creationIndex, new TestComponentA(), new TestComponentB(), new TestComponentC());
        }

        [Test]
        public void increments_creation_index()
        {
            var pool = TestPool();
            Assert.AreEqual(0, pool.CreateEntity().Id);
            Assert.AreEqual(1, pool.CreateEntity().Id);
        }

        [Test]
        public void start_with_given_creation_index()
        {
            Assert.AreEqual(42, TestPool(42).CreateEntity().Id);
        }

        [Test]
        public void must_has_no_entites()
        {
            Assert.IsEmpty(TestPool().GetEntities());
        }

        [Test]
        public void entity_creates()
        {
            Assert.IsInstanceOf<Entity<ITestPool>>(TestPool().CreateEntity());
        }

        [Test]
        public void creates_components_pool()
        {
            var pool = TestPool();
            Assert.IsNotNull(pool.ComponentPools);
            Assert.AreEqual(3, pool.ComponentPools.Count);
        }

        [Test]
        public void test_components_pool_injection()
        {
            var pool = TestPool();
            Assert.IsNotNull(pool.ComponentPools);
            var entity = pool.CreateEntity();

            Assert.AreSame(pool.ComponentPools, entity.ComponentsPool);

            entity.Add<TestComponentA>().Remove<TestComponentA>();

            Assert.AreSame(pool.ComponentPools, entity.ComponentsPool);
        }

        [Test]
        public void error_then_destroying_entity_which_pool_doesnt_contain()
        {
            var pool = TestPool();
            var e = pool.CreateEntity();
            pool.DestroyEntity(e);
            Assert.Throws<PoolDoesNotContainEntityException<ITestPool>>(() =>
                pool.DestroyEntity(e));
        }

        [Test]
        public void create_and_get_entity_count()
        {
            var p = TestPool();
            var e = p.CreateEntity().Add<TestComponentC>();

            Assert.AreEqual(1, p.Count);
        }

        [Test]
        public void has_entity_that_created()
        {
            var p = TestPool();
            var e = p.CreateEntity().Add<TestComponentC>();

            Assert.IsTrue(p.HasEntity(e));
        }

        [Test]
        public void doesnt_have_entity_created_another_pool()
        {
            var p = TestPool();
            var p2 = TestPool();
            var e = p.CreateEntity().Add<TestComponentC>();

            Assert.IsFalse(p2.HasEntity(e));
        }

        [Test]
        public void returns_all_created_entities()
        {
            var p = TestPool();
            var e1 = p.CreateEntity().Add<TestComponentC>();
            var e2 = p.CreateEntity().Add<TestComponentB>();
            var e3 = p.CreateEntity().Add<TestComponentA>();

            var entities = p.GetEntities();

            Assert.AreEqual(3, entities.Length);
            Assert.Contains(e1, entities);
            Assert.Contains(e2, entities);
            Assert.Contains(e3, entities);
        }

        [Test]
        public void destroys_entity_and_removes_it()
        {
            var p = TestPool();
            var e1 = p.CreateEntity().Add<TestComponentC>();

            p.DestroyEntity(e1);
            Assert.IsFalse(p.HasEntity(e1));
            Assert.AreEqual(0, p.Count);
            Assert.IsEmpty(p.GetEntities());
        }

        [Test]
        public void destroid_entity_shouldnt_have_components()
        {
            var p = TestPool();
            var e1 = p.CreateEntity().Add<TestComponentC>();

            Assert.AreEqual(1, e1.GetComponents().Length);

            p.DestroyEntity(e1);

            Assert.AreEqual(0, e1.GetComponents().Length);
        }

        [Test]
        public void destroys_all_entities()
        {
            var p = TestPool();
            var e1 = p.CreateEntity().Add<TestComponentC>();
            p.DestroyAllEntities();

            Assert.IsFalse(p.HasEntity(e1));
            Assert.AreEqual(0, p.Count);
            Assert.IsEmpty(p.GetEntities());

            Assert.IsEmpty(e1.GetComponents());
        }

        [Test]
        public void ensures_same_order_when_getting_entities_after_destroying()
        {
            var p = TestPool();

            for (int i = 0; i < 10; i++)
            {
                p.CreateEntity();
            }

            var order1 = new int[10];
            var entities1 = p.GetEntities();

            for (int i = 0; i < 10; i++)
            {
                order1[i] = entities1[i].Id;
            }

            p.DestroyAllEntities();
            p.ResetCreationIndex();



            for (int i = 0; i < 10; i++)
            {
                p.CreateEntity();
            }

            var order2 = new int[10];
            var entities2 = p.GetEntities();

            for (int i = 0; i < 10; i++)
            {
                order2[i] = entities2[i].Id;
            }

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(order1[i], order2[i]);
            }
        }

        [Test]
        public void throws_retain_exception_for_retained_entities_when_destroyed_it()
        {
            var p = TestPool();
            var e1 = p.CreateEntity().Add<TestComponentC>();
            e1.Retain(this);

            Assert.Throws<PoolStillHasRetainedEntitiesException<ITestPool>>(() => p.DestroyAllEntities());
        }

        [Test]
        public void caches_entity()
        {
            var p = TestPool();
            var e1 = p.CreateEntity().Add<TestComponentC>();

            var entities = p.GetEntities();

            Assert.AreSame(entities, p.GetEntities());
        }

        [Test]
        public void updates_entities_cached_when_creatin_entity()
        {
            var p = TestPool();
            var e = p.CreateEntity().Add<TestComponentC>();

            var entities = p.GetEntities();

            p.DestroyEntity(e);
            Assert.AreNotSame(entities, p.GetEntities());

            var entities2 = p.GetEntities();
            p.CreateEntity().Add<TestComponentC>();

            Assert.AreNotSame(entities2, p.GetEntities());
        }

        [Test]
        public void reset_all_events_handles()
        {
            var p = TestPool();

            p.OnEntityCreated += delegate { Assert.Fail(); };
            p.OnEntityWillBeDestroyed += delegate { Assert.Fail(); };
            p.OnEntityDestroyed += delegate { Assert.Fail(); };
            p.OnGroupCreated += delegate { Assert.Fail(); };
            p.OnGroupCleared += delegate { Assert.Fail(); };

            p.Reset();

            p.DestroyEntity(p.CreateEntity());
            p.GetGroup(Matcher.AllOf(typeof (TestComponentA)));
            p.ClearGroups();
        }

        [Test]
        public void dispatches_OnEntityCreated_when_creating_entity()
        {
            var did = 0;
            Entity<ITestPool> eventEntity = null;
            var pool = TestPool();
            pool.OnEntityCreated += (p, e) =>
            {
                did += 1;
                eventEntity = e;
                Assert.AreSame(pool, p);
            };


            var entity = pool.CreateEntity();

            Assert.AreEqual(1, did);
            Assert.AreSame(entity, eventEntity);
        }

        [Test]
        public void dispatched_OnEntityWillBeDestroyed_when_destroy()
        {
            var did = 0;
            var pool = TestPool();
            var entity = pool.CreateEntity();

            entity.Add<TestComponentA>();

            pool.OnEntityWillBeDestroyed += (p, e) =>
            {
                did += 1;
                Assert.AreSame(p, pool);
                Assert.AreSame(e, entity);

                Assert.IsTrue(e.Has<TestComponentA>());
                Assert.IsTrue(e.IsEnabled);

                Assert.AreEqual(0, p.GetEntities().Length);
            };

            pool.GetEntities();
            pool.DestroyEntity(entity);

            Assert.AreEqual(1, did);
        }

        [Test]
        public void dispatched_OnEntityDestroyed_wned_destroying()
        {
            var did = 0;
            var pool = TestPool();
            var entity = pool.CreateEntity();

            entity.Add<TestComponentA>();

            pool.OnEntityDestroyed += (p, e) =>
            {
                did += 1;
                Assert.AreSame(p, pool);
                Assert.AreSame(e, entity);

                Assert.IsFalse(e.Has<TestComponentA>());
                Assert.IsFalse(e.IsEnabled);
            };

            pool.DestroyEntity(entity);
            Assert.AreEqual(1, did);
        }

        [Test]
        public void entity_is_released_after_OnEntityDestroyed()
        {
            var did = 0;
            var pool = TestPool();
            var entity = pool.CreateEntity();

            pool.OnEntityDestroyed += (p, e) =>
            {
                did += 1;
                Assert.AreEqual(1, e.owners.Count);
                var ne = p.CreateEntity();
                Assert.IsNotNull(ne);
                Assert.AreNotSame(e, ne);
            };

            pool.DestroyEntity(entity);
            Assert.AreEqual(1, did);
            Assert.AreEqual(0, entity.owners.Count);
        }

        [Test]
        public void throws_if_entity_is_released_before_destroy()
        {
            var pool = TestPool();
            var entity = pool.CreateEntity();

            Assert.Throws<EntityIsNotDestroyedException<ITestPool>>(() => { entity.Release(pool); });

        }

        [Test]
        public void dispatched_OnGroupCreated_when_creating_a_new_group()
        {
            Group<ITestPool> eg = null;
            var did = 0;
            var pool = TestPool();

            pool.OnGroupCreated += (p, g) =>
            {
                did += 1;
                Assert.AreSame(p, pool);
                eg = g;
            };

            var group = pool.GetGroup(Matcher.AllOf(typeof (TestComponentA)));
            Assert.AreEqual(1, did);
            Assert.AreSame(eg, group);
        }

        [Test]
        public void doesnt_dispatch_OnGroupCreated_when_group_already_exists()
        {
            var pool = TestPool();
            pool.GetGroup(Matcher.AllOf(typeof (TestComponentA)));

            pool.OnGroupCreated += delegate { Assert.Fail(); };

            pool.GetGroup(Matcher.AllOf(typeof(TestComponentA)));
        }

        [Test]
        public void dispatch_OnGroupCleared_when_clearing_groups()
        {
            Group<ITestPool> eg = null;
            var did = 0;
            var pool = TestPool();
            pool.OnGroupCleared += (p, g) =>
            {
                did += 1;
                Assert.AreSame(p, pool);
                eg = g;
            };


            pool.GetGroup(Matcher.AllOf(typeof(TestComponentA)));
            var group2 = pool.GetGroup(Matcher.AllOf(typeof(TestComponentB)));
            pool.ClearGroups();

            Assert.AreEqual(2, did);
            Assert.AreSame(eg, group2);
        }

        [Test]
        public void removes_all_delegates_when_destroying_entity()
        {
            var pool = TestPool();
            var entity = pool.CreateEntity();

            entity.OnComponentAdded += delegate { Assert.Fail(" On Component Added fires"); };
            entity.OnComponentRemoved += delegate { Assert.Fail("On Component Removed fires"); };
            entity.OnComponentReplaced += delegate { Assert.Fail("On Component Replaced fires"); };

            pool.DestroyEntity(entity);

            var e2 = pool.CreateEntity();

            Assert.AreSame(e2, entity);
            e2.Add<TestComponentA>();
            e2.Replace<TestComponentA>();
            e2.Remove<TestComponentA>();
        }

        [Test]
        public void will_not_remove_external_delegates_for_OnEntityReleased()
        {
            var pool = TestPool();
            var entity = pool.CreateEntity();
            var did = 0;
            entity.OnEntityReleased += e => did += 1;
            pool.DestroyEntity(entity);
            Assert.AreEqual(1, did);
        }

        [Test]
        public void removes_all_delegates_from_OnEntityReleased_when_being_dispatched()
        {
            var pool = TestPool();
            var entity = pool.CreateEntity();
            var did = 0;
             
            entity.OnEntityReleased += e => did += 1;

            pool.DestroyEntity(entity);
            entity.Retain(this);
            entity.Release(this);

            Assert.AreEqual(1, did);
        }

        [Test]
        public void removes_all_delegates_from_OnEntityReleased_when_delayed_release()
        {
            var pool = TestPool();
            var entity = pool.CreateEntity();
            var did = 0;
            entity.OnEntityReleased += e => did += 1;

            entity.Retain(this);
            pool.DestroyEntity(entity);
            Assert.AreEqual(0, did);

            entity.Release(this);

            Assert.AreEqual(1, did);

            entity.Retain(this);
            entity.Release(this);

            Assert.AreEqual(1, did);
        }


        [Test]
        public void returns_pushed_entity()
        {
            var p1 = TestPool();
            var e1 = p1.CreateEntity();
            e1.Retain(this);
            p1.DestroyEntity(e1);

            Assert.AreNotSame(e1, p1.CreateEntity());
            e1.Release(this);

            var e2 = p1.CreateEntity();

            Assert.AreSame(e1, e2);
        }

        [Test]
        public void returns_new_entity()
        {
            var p1 = TestPool();
            var e1 = p1.CreateEntity();
            e1.Add<TestComponentA>();

            p1.DestroyEntity(e1);
            p1.CreateEntity();

            var e2 = p1.CreateEntity();

            Assert.IsFalse(e2.Has<TestComponentA>());
            Assert.AreNotSame(e1, e2);
        }

        [Test]
        public void group_must_contain_entity()
        {
            var p1 = TestPool();
            var e1 = p1.CreateEntity();
            e1.Add<TestComponentA>();

            var g = p1.GetGroup(Matcher.AllOf(typeof (TestComponentA)));

            Assert.Contains(e1, g.GetEntities());
        }

        [Test]
        public void throw_when_work_with_destroyed_entity()
        {
            var p1 = TestPool();
            var e1 = p1.CreateEntity();

            p1.DestroyEntity(e1);

            Assert.Throws<EntityIsNotEnabledException<ITestPool>>(() => e1.Add<TestComponentA>());
            Assert.Throws<EntityIsNotEnabledException<ITestPool>>(() => e1.Remove<TestComponentA>());
            Assert.Throws<EntityIsNotEnabledException<ITestPool>>(() => e1.Add<TestComponentA>());
        }
    }
}