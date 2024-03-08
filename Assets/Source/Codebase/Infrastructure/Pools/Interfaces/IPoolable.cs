namespace Source.Codebase.Infrastructure.Pools.Interfaces
{
    public interface IPoolable
    {
        void Init<T>(IPool<T> pool) where T : IPoolable;
        void Enable();
        void Disable();
        void OnReleaseToPool();
    }
}