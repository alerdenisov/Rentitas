using Rentitas.Tests.Extra;

namespace Rentitas.Tests
{
    public class TestApplication : Application
    {
    }

    public class TestKernelA : IKernel
    {
        public InitializeSystemSpy InitializeSystem { get; private set; }
        public DeinitializeSystemSpy DeinitializeSystem { get; private set; }
        public ExecuteSystemSpy ExecuteSystem { get; private set; }
        public CleanupSystemSpy CleanupSystem { get; private set; }

        public ReactiveSystem<ITestPool> ReactiveSystem { get; private set; }

        public Pool<ITestPool> Pool { get; private set; }

        public BaseScenario Scenario { get; private set; }
        public IPool[] PoolInterfaces { get; private set; }

        public TestKernelA()
        {
            Pool = new Pool<ITestPool>(new TestComponentA(), new TestComponentB(), new TestComponentC());

            InitializeSystem = new InitializeSystemSpy();
            DeinitializeSystem = new DeinitializeSystemSpy();
            ExecuteSystem = new ExecuteSystemSpy(); 
            CleanupSystem = new CleanupSystemSpy();

            var subSystem = new ReactiveSubSystemSpy(Matcher.AllOf(typeof(TestComponentA)), GroupEventType.OnEntityAdded);
            ReactiveSystem = new ReactiveSystem<ITestPool>(Pool, subSystem);

            Scenario = new BaseScenario("Test A")
                .Add(InitializeSystem)
                .Add(DeinitializeSystem)
                .Add(ExecuteSystem)
                .Add(CleanupSystem)
                .Add(ReactiveSystem);

            PoolInterfaces = new IPool[] { Pool };
        }
    }

    public class TestKernelB : IKernel
    {
        public PoolsSystemSpy PoolsSystem { get; private set; }
        public ApplicationSystemSpy ApplicationSystem { get; private set; }
        public Pool<ITestPool> SecondCore { get; private set; }
        public Pool<ITestSecondPool> SecondPool { get; private set; }
        public BaseScenario Scenario { get; private set; }
        public IPool[] PoolInterfaces { get; private set; }

        public TestKernelB()
        {
            SecondCore = new Pool<ITestPool>("SecondCore", new TestComponentA());
            SecondPool = new Pool<ITestSecondPool>(new TestNameComponent());
            ApplicationSystem = new ApplicationSystemSpy();
            PoolsSystem = new PoolsSystemSpy();

            Scenario = new BaseScenario("Test A").Add(ApplicationSystem).Add(PoolsSystem);

            PoolInterfaces = new IPool[] {SecondCore, SecondPool};
        }
    }
}