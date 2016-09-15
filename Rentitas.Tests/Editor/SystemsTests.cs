using JetBrains.Annotations;
using NUnit.Framework;
using Rentitas.Tests.Extra;

namespace Rentitas.Tests
{
    [TestFixture]
    public class BaseSystemsContext
    {
        protected Pool<ITestPool> pool;

        [SetUp]
        public void Setup1()
        {
            pool = new Pool<ITestPool>(0, new TestComponentA(), new TestComponentB(), new TestComponentC());
        }


        protected ReactiveSystem<ITestPool> CreateReactiveSystem(Pool<ITestPool> pool)
        {
            var subSystem = new ReactiveSubSystemSpy(Matcher.AllOf(typeof(TestComponentA)), GroupEventType.OnEntityAdded);
            var reactiveSystem = new ReactiveSystem<ITestPool>(pool, subSystem);
            pool.CreateEntity().Add<TestComponentA>();

            return reactiveSystem;
        }
    }

    [TestFixture]
    public class ScenarioContext : BaseSystemsContext
    {
        protected BaseScenario scenario;

        [SetUp]
        public void Setup2()
        {
            scenario = new BaseScenario();
        }
    }

    public class SystemsTests : BaseSystemsContext
    {
        [Test]
        public void initialize_spy_system()
        {
            var initSystem = new InitializeSystemSpy();
            Assert.AreEqual(0,initSystem.didInitialize);
            initSystem.Initialize();
            Assert.AreEqual(1, initSystem.didInitialize);
        }

        [Test]
        public void execute_spy_system()
        {
            var sys = new ExecuteSystemSpy();
            Assert.AreEqual(sys.didExecute, 0);
            sys.Execute();
            Assert.AreEqual(sys.didExecute, 1);
            sys.Execute();
            Assert.AreEqual(sys.didExecute, 2);
        }

        [Test]
        public void deinitialize_spy_systme()
        {
            var s = new DeinitializeSystemSpy();
            Assert.AreEqual(s.didDeinitialize, 0);
            s.Deinitialize();
            Assert.AreEqual(s.didDeinitialize, 1);
        }

        [Test]
        public void anything_spy_system()
        {
            var system = new InitializeExecuteCleanupDeinitializeSystemSpy();

            Assert.AreEqual(0, system.didInitialize);
            system.Initialize();
            Assert.AreEqual(1, system.didInitialize);

            Assert.AreEqual(0, system.didExecute);
            system.Execute();
            Assert.AreEqual(1, system.didExecute);

            Assert.AreEqual(0, system.didCleanup);
            system.Cleanup();
            Assert.AreEqual(1, system.didCleanup);

            Assert.AreEqual(0, system.didDeinitialize);
            system.Deinitialize();
            Assert.AreEqual(1, system.didDeinitialize);
        }

        [Test]
        public void executes_reactive_spy()
        {
            var sys = CreateReactiveSystem(pool);
            var spy = (ReactiveSubSystemSpy) sys.Subsystem;

            sys.Execute();

            Assert.AreEqual(1, spy.entities.Length);
        }
    }

    public class ScenarionTests : ScenarioContext
    {
        [Test]
        public void returns_scenario_when_adding_system()
        {
            Assert.AreSame(scenario, scenario.Add(new InitializeSystemSpy()));
        }

        [Test]
        public void insure_scenario_initialize_systems()
        {
            var s = new InitializeSystemSpy();
            scenario.Add(s);
            scenario.Initialize();
            Assert.AreEqual(1, s.didInitialize);
        }

        [Test]
        public void insure_scenario_execute_systems()
        {
            var s = new ExecuteSystemSpy();
            scenario.Add(s);
            scenario.Execute();
            scenario.Execute();
            scenario.Execute();

            Assert.AreEqual(3, s.didExecute);
        }

        [Test]
        public void insure_scenario_cleanup_systems()
        {
            var s = new CleanupSystemSpy();
            scenario.Add(s);
            scenario.Cleanup();
            Assert.AreEqual(1, s.didCleanup);
        }

        [Test]
        public void insure_scenario_init_execute_clean_and_deinit_systems()
        {
            var s = new InitializeExecuteCleanupDeinitializeSystemSpy();

            scenario.Add(s);

            Assert.AreEqual(0, s.didInitialize);
            scenario.Initialize();
            Assert.AreEqual(1, s.didInitialize);

            Assert.AreEqual(0, s.didExecute);
            scenario.Execute();
            Assert.AreEqual(1, s.didExecute);

            Assert.AreEqual(0, s.didCleanup);
            scenario.Cleanup();
            Assert.AreEqual(1, s.didCleanup);

            Assert.AreEqual(0, s.didDeinitialize);
            scenario.Deinitialize();
            Assert.AreEqual(1, s.didDeinitialize);
        }

        [Test]
        public void insure_scenario_init_execute_clean_deinit_reactive_sysmte()
        {
            var sys = CreateReactiveSystem(pool);
            var s = (ReactiveSubSystemSpy) sys.Subsystem;

            scenario.Add(sys);

            Assert.AreEqual(0, s.didInitialize);
            scenario.Initialize();
            Assert.AreEqual(1, s.didInitialize);

            Assert.AreEqual(0, s.didExecute);
            scenario.Execute();
            Assert.AreEqual(1, s.didExecute);

            Assert.AreEqual(0, s.didCleanup);
            scenario.Cleanup();
            Assert.AreEqual(1, s.didCleanup);

            Assert.AreEqual(0, s.didDeinitialize);
            scenario.Deinitialize();
            Assert.AreEqual(1, s.didDeinitialize);
        }

        [Test]
        public void init_execute_clean_deinit_systems_recursively()
        {
            var sys = CreateReactiveSystem(pool);
            var s = (ReactiveSubSystemSpy) sys.Subsystem;

            scenario.Add(sys);

            var parent = new BaseScenario();
            parent.Add(scenario);

            Assert.AreEqual(0, s.didInitialize);
            parent.Initialize();
            Assert.AreEqual(1, s.didInitialize);

            Assert.AreEqual(0, s.didExecute);
            parent.Execute();
            Assert.AreEqual(1, s.didExecute);

            Assert.AreEqual(0, s.didCleanup);
            parent.Cleanup();
            Assert.AreEqual(1, s.didCleanup);

            Assert.AreEqual(0, s.didDeinitialize);
            parent.Deinitialize();
            Assert.AreEqual(1, s.didDeinitialize);

        }

        [Test]
        public void clears_reactive_systems()
        {
            var sys = CreateReactiveSystem(pool);
            var s = (ReactiveSubSystemSpy) sys.Subsystem;

            scenario.Add(sys);

            scenario.Initialize();
            Assert.AreEqual(1, s.didInitialize);

            scenario.ClearReactiveSystems();
            scenario.Execute();

            Assert.AreEqual(0, s.didExecute);
        }

        [Test]
        public void clears_reactive_systems_recursively()
        {
            var sys = CreateReactiveSystem(pool);
            var s = (ReactiveSubSystemSpy) sys.Subsystem;

            scenario.Add(sys);

            var parent = new BaseScenario();
            parent.Add(scenario);


            parent.Initialize();
            Assert.AreEqual(1, s.didInitialize);

            parent.ClearReactiveSystems();

            parent.Execute();
            Assert.AreEqual(0, s.didExecute);
        }

        [Test]
        public void deactivates_reactive_systems()
        {
            var sys = CreateReactiveSystem(pool);
            var s = (ReactiveSubSystemSpy) sys.Subsystem;

            scenario.Add(sys);

            scenario.Initialize();
            Assert.AreEqual(1, s.didInitialize);

            scenario.DeactivateReactiveSystems();
            scenario.Execute();

            Assert.AreEqual(0, s.didExecute);
        }

        [Test]
        public void deactivate_reactive_systems_recursive()
        {
            var sys = CreateReactiveSystem(pool);
            var s = (ReactiveSubSystemSpy) sys.Subsystem;

            scenario.Add(sys);

            var parent = new BaseScenario();
            parent.Add(scenario);


            parent.Initialize();
            Assert.AreEqual(1, s.didInitialize);

            parent.DeactivateReactiveSystems();

            parent.Execute();
            Assert.AreEqual(0, s.didExecute);

        }

        [Test]
        public void activate_reactive_systems()
        {

            var sys = CreateReactiveSystem(pool);
            var s = (ReactiveSubSystemSpy) sys.Subsystem;

            scenario.Add(sys);

            scenario.Initialize();
            Assert.AreEqual(1, s.didInitialize);

            scenario.DeactivateReactiveSystems();
            scenario.Execute();

            Assert.AreEqual(0, s.didExecute);

            scenario.ActivateReactiveSystems();
            scenario.Execute();
            Assert.AreEqual(0, s.didExecute);

            pool.CreateEntity().Add<TestComponentA>();
            scenario.Execute();
            Assert.AreEqual(1, s.didExecute);
        }

        [Test]
        public void activate_reactive_systems_recursive()
        {
            var sys = CreateReactiveSystem(pool);
            var s = (ReactiveSubSystemSpy) sys.Subsystem;

            scenario.Add(sys);

            var parent = new BaseScenario();
            parent.Add(scenario);


            parent.Initialize();
            Assert.AreEqual(1, s.didInitialize);

            parent.DeactivateReactiveSystems();

            parent.Execute();
            Assert.AreEqual(0, s.didExecute);

            parent.ActivateReactiveSystems();
            parent.Execute();
            Assert.AreEqual(0, s.didExecute);

            pool.CreateEntity().Add<TestComponentA>();
            parent.Execute();
            Assert.AreEqual(1, s.didExecute);

        }

    }
}