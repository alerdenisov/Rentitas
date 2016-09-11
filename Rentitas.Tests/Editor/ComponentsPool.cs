using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Rentitas.Tests
{
    [TestFixture]
    public class ComponentsPool
    {
        Pool<ITestPool> pool;

        private Pool<ITestPool> TestPool(int creationIndex = 0)
        {
            return new Pool<ITestPool>(creationIndex, new TestComponentA(), new TestComponentB(), new TestComponentC());
        }

        [SetUp]
        public void SetupContext()
        {
            pool = TestPool();
            pool.CreateEntity()
                .Add<TestComponentA>().Add<TestComponentB>()
                .Remove<TestComponentA>().Remove<TestComponentB>();
        }

        [Test]
        public void clears_all_component_pools()
        {
            Debug.Log(pool.ComponentPools);
            Debug.Log(string.Join(", ", pool.ComponentPools.Keys.Select(t => t.ToString()).ToArray()));
            Assert.AreEqual(1, pool.ComponentPools[typeof(TestComponentA)].Count);
            Assert.AreEqual(1, pool.ComponentPools[typeof(TestComponentB)].Count);

            pool.ClearComponentPools();

            Assert.AreEqual(0, pool.ComponentPools[typeof(TestComponentA)].Count);
            Assert.AreEqual(0, pool.ComponentPools[typeof(TestComponentB)].Count);
        }

        [Test]
        public void clears_specific_component_pool()
        {
            pool.ClearComponentPool(typeof(TestComponentA));

            Assert.AreEqual(0, pool.ComponentPools[typeof(TestComponentA)].Count);
            Assert.AreEqual(1, pool.ComponentPools[typeof(TestComponentB)].Count);
        }
    }
}