namespace Rentitas.Tests
{
    public interface ITestPool : IComponent { }
    public interface ITestSecondPool : IComponent { }

    public class TestComponentA : ITestPool { }
    public class TestComponentB : ITestPool { }
    public class TestComponentC : ITestPool { }

    public class NameTestComponent : ITestSecondPool
    {
        public string Name;
    }

    public class IdTestComponent : ITestSecondPool
    {
        public int Id;
    }

}