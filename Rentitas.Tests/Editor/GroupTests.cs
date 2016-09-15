using System.Linq;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace Rentitas.Tests
{
    [TestFixture]
    public class GroupTests
    {
        Entity<ITestPool> eAB1;
        Entity<ITestPool> eAB2;
        Entity<ITestPool> eA;

        IMatcher matcherAB;

        Pool<ITestPool> pool;

        private Pool<ITestPool> TestPool(int creationIndex = 0)
        {
            return new Pool<ITestPool>(string.Empty, creationIndex, new TestComponentA(), new TestComponentB(), new TestComponentC());
        }


        [SetUp]
        public void Setup()
        {
            pool = TestPool();
            matcherAB = Matcher.AllOf(typeof(TestComponentA), typeof(TestComponentB));

            eAB1 = pool.CreateEntity();
            eAB1.Add<TestComponentA>().Add<TestComponentB>();
            eAB2 = pool.CreateEntity();
            eAB2.Add<TestComponentA>().Add<TestComponentB>();

            eA = pool.CreateEntity();
            eA.Add<TestComponentA>();
        }

        [Test]
        public void gets_group_with_matching_entities()
        {
            var g = pool.GetGroup(matcherAB).GetEntities();
            Assert.AreEqual(2, g.Length);
            Assert.Contains(eAB1, g);
            Assert.Contains(eAB2, g);
        }

        [Test]
        public void gets_cached_group()
        {
            Assert.AreSame(pool.GetGroup(matcherAB), pool.GetGroup(matcherAB));
        }

        [Test]
        public void cached_group_contains_newly_created_entity()
        {
            var g = pool.GetGroup(matcherAB);
            eA.Add<TestComponentB>();
            Assert.Contains(eA, g.GetEntities());
        }

        [Test]
        public void cached_group_doesnt_contain_entity_which_are_not_matching_anymore()
        {
            var g = pool.GetGroup(matcherAB);
            eAB1.Remove<TestComponentB>();

            Assert.IsFalse(g.GetEntities().Contains(eAB1));
        }

        [Test]
        public void removes_destroyed_entity_from_group()
        {
            var g = pool.GetGroup(matcherAB);

            pool.DestroyEntity(eAB1);

            Assert.IsFalse(g.GetEntities().Contains(eAB1));
        }

        [Test]
        public void group_dispatches_OnEntityRemoved_and_OnEntityAdded_when_replacing_components()
        {
            var g = pool.GetGroup(matcherAB);
            var didR = 0;
            var didA = 0;
            var compP = eAB1.Get(typeof (TestComponentA));
            var compA = new TestComponentA();

            g.OnEntityRemoved += (grp, entity, type, comp) =>
            {
                Assert.AreSame(g, grp);
                Assert.AreSame(entity, eAB1);
                Assert.AreEqual(typeof (TestComponentA), type);
                Assert.AreSame(comp, compP);
                didR += 1;
            };

            g.OnEntityAdded += (grp, entity, type, comp) =>
            {
                Assert.AreSame(g, grp);
                Assert.AreSame(entity, eAB1);
                Assert.AreEqual(typeof (TestComponentA), type);
                Assert.AreSame(comp, compA);
                didA += 1;
            };

            eAB1.ReplaceInstance(compA);

            Assert.AreEqual(1, didA);
            Assert.AreEqual(1, didR);
        }

        [Test]
        public void group_dispatch_OnEntityUpdated_with_previous_and_current_components()
        {
            var update = 0;
            var prev = eAB1.Get<TestComponentA>();
            var next = new TestComponentA();
            var g = pool.GetGroup(matcherAB);

            g.OnEntityUpdated += (grp, e, type, p, n) =>
            {
                Assert.AreSame(g, grp);
                Assert.AreSame(e, eAB1);
                Assert.AreEqual(typeof (TestComponentA), type);
                Assert.AreSame(p, prev);
                Assert.AreSame(n, next);

                update += 1;
            };

            eAB1.ReplaceInstance(next);

            Assert.AreEqual(1, update);
        }

        [Test]
        public void dispatches_OnEntityAdded_after_grp_updated()
        {
            var g1 = pool.GetGroup(matcherAB);
            var g2 = pool.GetGroup(Matcher.AllOf(typeof (TestComponentB)));

            g1.OnEntityUpdated += delegate { Assert.AreEqual(3, g2.Count); };

            var e = pool.CreateEntity().Add<TestComponentA>().Add<TestComponentB>();
        }
        [Test]
        public void dispatches_OnEntityRemoved_after_grp_updated()
        {
            var g1 = pool.GetGroup(matcherAB);
            var g2 = pool.GetGroup(Matcher.AllOf(typeof (TestComponentB)));

            g1.OnEntityRemoved += delegate { Assert.AreEqual(1, g2.Count); };

            eAB2.Remove<TestComponentB>();
        }

        [Test]
        public void reset_and_remove_groups_from_pool()
        {
            var created = 0;
            Group<ITestPool> createdGroup = null;
            pool.OnGroupCreated += (p, g) =>
            {
                created += 1;
                createdGroup = g;
            };

            var initiategroup = pool.GetGroup(matcherAB);

            pool.ClearGroups();

            pool.GetGroup(matcherAB);

            pool.CreateEntity().Add<TestComponentA>().Add<TestComponentB>();

            Assert.AreEqual(created, 2);
            Assert.IsNotNull(createdGroup);
            Assert.AreNotSame(createdGroup, initiategroup);

            Assert.AreEqual(2, initiategroup.Count);
            Assert.AreEqual(3, createdGroup.Count);
        }

        [Test]
        public void removes_all_event_handlers_from_Groups()
        {
            var group = pool.GetGroup(matcherAB);
            group.OnEntityAdded += delegate { Assert.Fail(); };

            pool.ClearGroups();

            var e = pool.CreateEntity();
            e.Add<TestComponentA>().Add<TestComponentB>();

            Assert.IsFalse(group.GetEntities().Contains(e));
            group.HandleEntity(e, typeof (TestComponentA), e.Get<TestComponentA>());
        }

        [Test]
        public void release_entities_in_groups()
        {
            var g = pool.GetGroup(matcherAB);
            Assert.AreEqual(2, eAB1.RetainCount);
            pool.ClearGroups();
            Assert.AreEqual(1, eAB1.RetainCount);
        }

        [Test]
        public void pops_new_list_from_list_pool()
        {
            pool.DestroyAllEntities();

            var gA = pool.GetGroup(Matcher.AllOf(typeof (TestComponentA)));
            var gAB = pool.GetGroup(Matcher.AnyOf(typeof (TestComponentA), typeof(TestComponentB)));
            var gABC = pool.GetGroup(Matcher.AnyOf(typeof (TestComponentA), typeof(TestComponentB), typeof(TestComponentC)));

            var did = 0;

            gA.OnEntityAdded += (grp, e, type, comp) =>
            {
                did += 1;
                e.Remove<TestComponentA>();
            };

            gAB.OnEntityAdded += delegate { did += 1; };
            gABC.OnEntityAdded += delegate { did += 1; };

            pool.CreateEntity().Add<TestComponentA>();

            Assert.AreEqual(3, did);
        }
    }
}