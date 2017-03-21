using NUnit.Framework;
using Rentitas;

namespace Rentitas.Tests.Indexes
{
    public class BaseIndexesContext 
    {
        protected Pool<ITestSecondPool> Pool;
        protected Group<ITestSecondPool> Group;
        protected PrimaryEntityIndex<ITestSecondPool, string> Index;

        [SetUp]
        public void Setup0()
        {
            Pool = new Pool<ITestSecondPool>(new NameTestComponent());
            Group = Pool.GetGroup(Matcher.AllOf(typeof(NameTestComponent)));
            Index = new PrimaryEntityIndex<ITestSecondPool, string>(Group, (e, c) =>
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
        protected Entity<ITestSecondPool> Entity = null;

        [SetUp]
        public void Setup1()
        {
            Entity = Pool.CreateEntity().Add<NameTestComponent>(n => n.Name = Key);
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

    [TestFixture]
    public class BaseIndexTests : BaseIndexesContext
    {
        [Test]
        public void should_doesnt_have_entity()
        {
            Assert.IsFalse(Index.HasEntity("unknownKey"));
        }

        [Test]
        public void throws_exception_when_attempting_to_get_entity()
        {
            Assert.Throws<EntityIndexException>(() =>
            {
                Index.GetEntity("unknownKey");
            });
        }

        [Test]
        public void return_null_when_trying_to_Get_entity()
        {
            Assert.IsNull(Index.TryGetEntity("unknownKey"));
        }
    }

    [TestFixture]
    public class KeyIndexTests : IndexWithNameContext
    {
        [Test]
        public void should_has_entity()
        {
            Assert.IsTrue(Index.HasEntity(Key));
        }

        [Test]
        public void gets_entity_via_key()
        {
            Assert.AreSame(Entity, Index.GetEntity(Key));
        }

        [Test]
        public void gets_entity_when_trying()
        {
            Assert.AreSame(Entity, Index.TryGetEntity(Key));
        }

        [Test]
        public void retains_entity()
        {
            Assert.AreEqual(3, Entity.RetainCount);
        }

        [Test]
        public void has_existing_entity()
        {
            var newIndex = new PrimaryEntityIndex<ITestSecondPool, string>(Group, (e, c) => {
                var nameAge = c as NameTestComponent;
                return nameAge != null
                        ? nameAge.Name
                        : e.Get<NameTestComponent>().Name;
            });

            Assert.IsTrue(newIndex.HasEntity(Key));
        }

        [Test]
        public void releases_and_removes_from_index()
        {
            Entity.Remove<NameTestComponent>();
            Assert.IsFalse(Index.HasEntity(Key));
            Assert.AreEqual(1, Entity.RetainCount);
        }

        [Test]
        public void throws_when_adding_an_entity_with_same_key()
        {
            Assert.Throws<EntityIndexException>(() =>
            {
                Pool.CreateEntity().Add<NameTestComponent>(n => n.Name = Key);
            });
        }
    }

    [TestFixture]
    public class DeactivatedIndexTests : DeactivatedIndexContext
    {
        [Test]
        public void clears_index_and_releases_entity()
        {
            Assert.IsFalse(Index.HasEntity(Key));
            Assert.AreEqual(2, Entity.RetainCount);
        }

        [Test]
        public void doesnt_add_entities_when_deactivated()
        {
            Pool.CreateEntity().Add<NameTestComponent>(n => n.Name = "Aler");
            Assert.IsFalse(Index.HasEntity("Aler"));
        }
    }

    [TestFixture]
    public class ReactivatedIndexTests : ReactivatedIndexContext
    {
        [Test]
        public void has_existing_entity()
        {
            Assert.IsTrue(Index.HasEntity(Key));
        }

        [Test]
        public void adds_new_entities()
        {
            var newKey = "Jack";
            Assert.IsFalse(Index.HasEntity(newKey));
            Pool.CreateEntity().Add<NameTestComponent>(n => n.Name = newKey);
            Assert.IsTrue(Index.HasEntity(newKey));
        }
    }
}
