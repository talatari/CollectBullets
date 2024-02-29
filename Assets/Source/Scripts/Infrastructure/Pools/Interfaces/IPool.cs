using System.Collections.Generic;

namespace Source.Scripts.Infrastructure.Pools.Interfaces
{
    // TODO: QUESTION: правильно ли я использовал тут Action?
    public delegate void CompletedAction();
    
    public interface IPool<T> where T : IPoolable
    {
        event CompletedAction Completed;
        
        int StartItemCount { get; }
        int AllItemsCount { get; }
        List<T> ActiveItems { get; }
        void Init();
        T Get();
        void Release(T item);
        void SetActive(T item);
    }
}