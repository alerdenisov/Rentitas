namespace Rentitas.Tests
{
    public class TestApplication : Application
    {
    }

    public class TestKernelA : IKernel
    {
        public BaseScenario Scenario { get; private set; }
        public IPool[] PoolInterfaces { get; private set; }

        public TestKernelA()
        {
            PoolInterfaces = new IPool[]
            {
                new Pool<ITestPool>(new TestComponentA(), new TestComponentB(), new TestComponentC())
            };
        }
    }

    public class TestKernelB : IKernel
    {
        public BaseScenario Scenario { get; private set; }
        public IPool[] PoolInterfaces { get; private set; }

        public TestKernelB()
        {
            PoolInterfaces = new IPool[]
            {
                new Pool<ITestPool>("SecondCore", new TestComponentA()),
                new Pool<ITestSecondPool>(new TestNameComponent())
            };
        }
    }
}