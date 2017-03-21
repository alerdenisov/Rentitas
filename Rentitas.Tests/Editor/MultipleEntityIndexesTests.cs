using NUnit.Framework;
using Rentitas;
using System.Linq;

namespace Rentitas.Tests.Indexes.Multiple
{
    public class BaseIndexesContext 
    {
        protected Pool<ITestSecondPool> Pool;
        protected Group<ITestSecondPool> Group;
        protected EntityIndex<ITestSecondPool, string> Index;

        [SetUp]
        public void Setup0()
        {
            Pool = new Pool<ITestSecondPool>(new NameTestComponent(), new IdTestComponent());
            Group = Pool.GetGroup(Matcher.AllOf(typeof(NameTestComponent)));
            Index = new EntityIndex<ITestSecondPool, string>(Group, (e, c) =>
            {
                var name = c as NameTestComponent;
                return name != null
                    ? name.Name
                    : e.Get<NameTestComponent>().Name;
            });
        }
    }

    public class IndexWithNameContext : BaseIndexesContext
    {
        protected const string Key = "Max";
        protected Entity<ITestSecondPool> Entity1 = null;
        protected Entity<ITestSecondPool> Entity2 = null;

        [SetUp]
        public void Setup1()
        {
            Entity1 = Pool.CreateEntity().Add<NameTestComponent>(n => n.Name = Key);
            Entity2 = Pool.CreateEntity().Add<NameTestComponent>(n => n.Name = Key);
        }
    }

    public class DeactivatedIndexContext : IndexWithNameContext
    {
        [SetUp]
        public void Setup2()
        {
            Index.Deactivate();
        }
    }
    public class ReactivatedIndexContext : DeactivatedIndexContext
    {
        [SetUp]
        public void Setup3()
        {
            Index.Activate();
        }
    }

    public class MultipleComponentIndex : ReactivatedIndexContext
    {
        protected Entity<ITestSecondPool> Entity3 = null;
        protected Entity<ITestSecondPool> Entity4 = null;

        protected Group<ITestSecondPool> GroupAll;
        protected Group<ITestSecondPool> GroupAny;
        protected Group<ITestSecondPool> GroupNone;

        protected EntityIndex<ITestSecondPool, string> IndexAll;
        protected EntityIndex<ITestSecondPool, string> IndexAny;
        protected EntityIndex<ITestSecondPool, int>    IndexNone;

        protected const string Key2 = "Jack";
        protected const int Id1 = 42;
        protected const int Id2 = 228;


        [SetUp]
        public void Setup4()
        {
            GroupAll = Pool.GetGroup(Matcher.AllOf(typeof(NameTestComponent), typeof(IdTestComponent)));
            GroupAny = Pool.GetGroup(Matcher.AnyOf(typeof(NameTestComponent), typeof(IdTestComponent)));
            GroupNone = Pool.GetGroup(Matcher.AllOf(typeof(IdTestComponent)).NoneOf(typeof(NameTestComponent)));

            Entity3 = Pool.CreateEntity().Add<IdTestComponent>(id => id.Id = Id1);
            Entity4 = Pool.CreateEntity().Add<NameTestComponent>(n => n.Name = Key2).Add<IdTestComponent>(id => id.Id = Id2);

            IndexAll = new EntityIndex<ITestSecondPool, string>(GroupAll, (e, c) => e.Get<NameTestComponent>().Name);
            IndexAny = new EntityIndex<ITestSecondPool, string>(GroupAny, (e, c) =>
            {
                if(e.Has<NameTestComponent>())
                {
                    return e.Get<NameTestComponent>().Name;
                }
                else
                {
                    return e.Get<IdTestComponent>().Id.ToString();
                }
            });

            IndexNone = new EntityIndex<ITestSecondPool, int>(GroupNone, (e, c) => e.Get<IdTestComponent>().Id);
        }
    }

    [TestFixture]
    public class BaseIndexTests : BaseIndexesContext
    {
        [Test]
        public void should_doesnt_have_entity()
        {
            Assert.IsFalse(Index.HasEntities("unknownKey"));
            Assert.IsEmpty(Index.GetEntities("unknownKey"));
        }
    }

    [TestFixture]
    public class KeyIndexTests : IndexWithNameContext
    {
        [Test]
        public void should_has_entity()
        {
            var entities = Index.GetEntities(Key);
            Assert.AreEqual(2, entities.Count);
            Assert.Contains(Entity1, entities.ToList());
            Assert.Contains(Entity2, entities.ToList());
        }

        [Test]
        public void retains_entity()
        {
            Assert.AreEqual(3, Entity1.RetainCount);
            Assert.AreEqual(3, Entity2.RetainCount);
        }

        [Test]
        public void has_existing_entity()
        {
            var newIndex = new EntityIndex<ITestSecondPool, string>(Group, (e, c) => {
                var nameAge = c as NameTestComponent;
                return nameAge != null
                        ? nameAge.Name
                        : e.Get<NameTestComponent>().Name;
            });

            Assert.IsTrue(newIndex.HasEntities(Key));
            Assert.AreEqual(2, newIndex.GetEntities(Key).Count);
        }

        [Test]
        public void releases_and_removes_from_index()
        {
            Entity1.Remove<NameTestComponent>();
            Assert.AreEqual(1, Index.GetEntities(Key).Count);
        }
    }

    [TestFixture]
    public class DeactivatedIndexTests : DeactivatedIndexContext
    {
        [Test]
        public void clears_index_and_releases_entity()
        {
            Assert.IsEmpty(Index.GetEntities(Key));
            Assert.AreEqual(2, Entity1.RetainCount);
            Assert.AreEqual(2, Entity2.RetainCount);
        }

        [Test]
        public void doesnt_add_entities_when_deactivated()
        {
            Pool.CreateEntity().Add<NameTestComponent>(n => n.Name = "Aler");
            Assert.IsEmpty(Index.GetEntities("Aler"));
        }
    }

    [TestFixture]
    public class ReactivatedIndexTests : ReactivatedIndexContext
    {
        [Test]
        public void has_existing_entity()
        {
            Assert.IsTrue(Index.HasEntities(Key));
            Assert.Contains(Entity1, Index.GetEntities(Key).ToList());
            Assert.Contains(Entity2, Index.GetEntities(Key).ToList());
        }

        [Test]
        public void adds_new_entities()
        {
            var newKey = "Jack";
            Assert.IsFalse(Index.HasEntities(newKey));
            Pool.CreateEntity().Add<NameTestComponent>(n => n.Name = newKey);
            Assert.IsTrue(Index.HasEntities(newKey));
        }

        [Test]
        public void join_new_entity()
        {
            Assert.AreEqual(2, Index.GetEntities(Key).Count);
            Pool.CreateEntity().Add<NameTestComponent>(n => n.Name = Key);
            Assert.AreEqual(3, Index.GetEntities(Key).Count);
        }
    }

    [TestFixture]
    public class MultipleComponentIndexTests : MultipleComponentIndex
    {
        [Test]
        public void allof_index_should_has_one_entity()
        {
            Assert.AreEqual(0, IndexAll.GetEntities(Key).Count);
            Assert.AreEqual(1, IndexAll.GetEntities(Key2).Count);
        }

        [Test]
        public void anyof_index_should_has_entities()
        {
            Assert.AreEqual(2, IndexAny.GetEntities(Key).Count);
            Assert.AreEqual(1, IndexAny.GetEntities(Key2).Count);
            Assert.AreEqual(1, IndexAny.GetEntities(Id1.ToString()).Count);
        }

        [Test]
        public void noneof_should_has_entites()
        {
            Assert.Contains(Entity3, IndexNone.GetEntities(Id1).ToList());
            Assert.IsEmpty(IndexNone.GetEntities(Id2).ToList());
        }
    }
}
