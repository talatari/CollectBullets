using System.Collections.Generic;

namespace Source.Scripts.Infrastructure.Pools.Interfaces
{
    public delegate void CompletedAction();
    
    public interface IPool<T> where T : IPoolable
    {
        event CompletedAction Completed;
        
        // TODO: добавить сюда Action Complited и подписываясь на него в SpawnerWave запускать некст волну
        int StartItemCount { get; }
        int AllItemsCount { get; }
        List<T> ActiveItems { get; }
        void Init();
        T Get();
        void Release(T item);
        void SetActive(T item);
    }
}