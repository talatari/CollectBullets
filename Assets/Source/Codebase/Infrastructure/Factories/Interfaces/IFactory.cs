using Source.Codebase.Infrastructure.Pools.Interfaces;

namespace Source.Codebase.Infrastructure.Factories.Interfaces
{
    public interface IFactory<T> where T : IPoolable
    {
        T Create();
    }
}