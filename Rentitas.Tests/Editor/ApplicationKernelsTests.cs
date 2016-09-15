using Rentitas.Tests;
using NUnit.Framework;

namespace Rentitas.Tests
{
    [TestFixture]
    public class BaseApplicationContext
    {
        protected IApplication app;

        [SetUp]
        public void Setup()
        {
            app = new TestApplication();
        }
    }

    [TestFixture]
    public class KernelApplicationContenxt : BaseApplicationContext
    {
        protected IKernel kernelA;

        [SetUp]
        public void Setup1()
        {
            kernelA = new TestKernelA();
        }
    }

    public class BothKernelsContext : KernelApplicationContenxt
    {
        protected IKernel kernelB;

        [SetUp]
        public void Setup2()
        {
            kernelB = new TestKernelB();
        }
    }

    public class RegisteredKernelsContext : BothKernelsContext
    {
        [SetUp]
        public void Setup3()
        {
            app.RegisterKernel(kernelA);
            app.RegisterKernel(kernelB);
        }
    }

    public class ApplicationKernelsTests : BaseApplicationContext
    {
        [Test]
        public void instance_of_application_is_available()
        {
            Assert.IsNotNull(app);
        }

        [Test]
        public void app_scenario_is_available()
        {
            Assert.IsNotNull(app.MainScenario);
        }
    }

    public class KernelTests : KernelApplicationContenxt
    {


        [Test]
        public void app_register_kernel()
        {
            Assert.AreEqual(typeof(ITestPool), kernelA.PoolInterfaces[0].PoolType);

            app.RegisterKernel(kernelA);
        }

        [Test]
        public void app_register_kernel_pool()
        {
            Assert.AreEqual(typeof(ITestPool), kernelA.PoolInterfaces[0].PoolType);

            app.RegisterKernel(kernelA);

            var registeredPool = app.Pools.Get<ITestPool>();
            Assert.AreEqual(registeredPool, kernelA.PoolInterfaces[0]);
        }
    }

    public class TwoKernelsTest : BothKernelsContext
    {
        [Test]
        public void register_same_pools_from_kernels()
        {
            Assert.AreEqual(kernelA.PoolInterfaces[0].PoolType, kernelB.PoolInterfaces[0].PoolType);

            app.RegisterKernel(kernelA);
            app.RegisterKernel(kernelB);
        }
    }
}