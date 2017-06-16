namespace Rentitas
{
    public interface IComponent {}

    /// <summary>
    /// Mark component as  
    /// </summary>
    public interface ISingleton {}

    public interface IFlag { }

    public interface IIndex<T> where T : struct
    {
        T GetIndex();
    }
}