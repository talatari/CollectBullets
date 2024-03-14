using System;
using System.Collections.Generic;

namespace Source.Codebase.Infrastructure.Pools.Interfaces
{
    public interface IPool<T> where T : IPoolable
    {
        event Action Completed;
        
        int StartItemCount { get; }
        int AllItemsCount { get; }
        List<T> ActiveItems { get; }
        
        void Init();
        T Get();
        void Release(T item);
        void SetActive(T item);
        void ReleaseAll();
        // void SetKeyCollected();
    }
}