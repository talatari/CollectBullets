namespace Source.Codebase.Infrastructure.Factories.Interfaces
{
    public interface IFactory<T>
    {
        T Create();
    }
}