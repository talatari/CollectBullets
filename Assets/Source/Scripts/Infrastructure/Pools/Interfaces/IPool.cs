using System.Collections.Generic;

namespace Source.Scripts.Infrastructure.Pools.Interfaces
{
    public interface IPool<T> where T : IPoolable
    {
        int StartItemCount { get; }
        int AllItemsCount { get; }
        List<T> ActiveItems { get; }
        void Init();
        T Get();
        void Release(T item);
        void SetActive(T item);
    }
}