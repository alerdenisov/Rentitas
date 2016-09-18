using Rentitas.Tests;
using NUnit.Framework;
using Rentitas.SampleApp;
using Rentitas.Tests.Extra;

namespace Rentitas.Tests.Applications
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
        protected TestKernelA kernelA;

        [SetUp]
        public void Setup1()
        {
            kernelA = new TestKernelA();
        }
    }

    public class BothKernelsContext : KernelApplicationContenxt
    {
        protected TestKernelB kernelB;

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

        [Test]
        public void should_kernel_initialize_system_after_registration()
        {
            var iniSystem = kernelA.InitializeSystem;
            Assert.AreEqual(iniSystem.didInitialize, 0);
            app.RegisterKernel(kernelA);
            Assert.AreEqual(iniSystem.didInitialize, 1);
        }

        [Test]
        public void kernel_systems_should_be_executed_after_execution()
        {
            var iniSystem = kernelA.ExecuteSystem;
            Assert.AreEqual(iniSystem.didExecute, 0);
            app.RegisterKernel(kernelA);
            Assert.AreEqual(iniSystem.didExecute, 0);
            app.Execute();
            Assert.AreEqual(iniSystem.didExecute, 1);
        }

        [Test]
        public void kernel_systems_should_be_deinitialized_after_unregistration()
        {
            var deinit = kernelA.DeinitializeSystem;
            Assert.AreEqual(deinit.didDeinitialize, 0);
            app.UnregisterKernel(kernelA);
            Assert.AreEqual(deinit.didDeinitialize, 1);
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

        [Test]
        public void pools_should_be_injected_after_registation()
        {
            var iniSystem = kernelB.PoolsSystem;
            Assert.AreEqual(iniSystem.didInject, 0);
            app.RegisterKernel(kernelB);
            Assert.AreEqual(iniSystem.didInject, 1);
            Assert.AreEqual(iniSystem.Pools, app.Pools);
        }

        [Test]
        public void app_should_be_injected_after_registration()
        {
            var appsys = kernelB.ApplicationSystem;
            Assert.AreEqual(appsys.didInject, 0);
            app.RegisterKernel(kernelB);
            Assert.AreEqual(appsys.didInject, 1);
            Assert.AreEqual(appsys.Application, app);
        }

        [Test]
        public void insure_app_init_execute_clean_deinit_reactive_sysmtems_in_kernel()
        {
            var sys = kernelA.ReactiveSystem;
            var s = (ReactiveSubSystemSpy) sys.Subsystem;

            Assert.AreEqual(0, s.didInitialize);
            app.RegisterKernel(kernelA);
            Assert.AreEqual(1, s.didInitialize);

            Assert.AreEqual(0, s.didExecute);
            app.Pools.Get<ITestPool>().CreateEntity().Add<TestComponentA>();
            Assert.AreEqual(0, s.didExecute);
            app.Execute();
            Assert.AreEqual(1, s.didExecute);
            app.Pools.Get<ITestPool>().CreateEntity().Add<TestComponentA>();
            app.Execute();
            Assert.AreEqual(2, s.didExecute);
            Assert.AreEqual(0, s.didCleanup);
            Assert.AreEqual(0, s.didDeinitialize);

            app.UnregisterKernel(kernelA);
            Assert.AreEqual(1, s.didCleanup);
            Assert.AreEqual(1, s.didDeinitialize);


            app.Pools.Get<ITestPool>().CreateEntity().Add<TestComponentA>();
            app.Execute();
            Assert.AreEqual(2, s.didExecute);
        }
    }

    public class RegisteredKernelsTest : RegisteredKernelsContext
    {
        [Test]
        public void returns_pools_by_name()
        {
            var core = app.Pools.Get<ITestPool>();
            var secondCore = app.Pools.Get<ITestPool>("SecondCore"); 

            Assert.AreNotSame(core, secondCore);
            Assert.AreSame(secondCore, kernelB.PoolInterfaces[0]);
        }


    }



}