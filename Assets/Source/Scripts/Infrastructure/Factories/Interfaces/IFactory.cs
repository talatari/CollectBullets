using Source.Scripts.Infrastructure.Pools.Interfaces;

namespace Source.Scripts.Infrastructure.Factories.Interfaces
{
    public interface IFactory<T> where T : IPoolable
    {
        T Create();
    }
}