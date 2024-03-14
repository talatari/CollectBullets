using System;
using System.Collections.Generic;
using Source.Codebase.Infrastructure.Factories.Interfaces;
using Source.Codebase.Infrastructure.Pools.Interfaces;
using Source.Codebase.Infrastructure.Services;

namespace Source.Codebase.Infrastructure.Pools
{
    public class Pool<T> : IPool<T> where T : IPoolable
    {
        private readonly IFactory<T> _factoryItems;
        private readonly int _startItemsCount;

        private Queue<T> _pool = new();
        private List<T> _activeItems = new();
        private GameLoopMediator _gameLoopMediator;

        public Pool(IFactory<T> factoryItems, int startItemsCount)
        {
            _factoryItems = factoryItems ?? throw new ArgumentNullException(nameof(factoryItems));
            
            if (startItemsCount <= 0) 
                throw new ArgumentOutOfRangeException(nameof(startItemsCount));
            
            _startItemsCount = startItemsCount;
        }

        public event Action Completed;

        public int StartItemCount => _startItemsCount;
        public int AllItemsCount => _activeItems.Count + _pool.Count;
        public List<T> ActiveItems => _activeItems;

        public void Init()
        {
            for (int i = 0; i < _startItemsCount; i++)
                Release(_factoryItems.Create());
        }

        public void ReleaseAll()
        {
            foreach (T item in _activeItems)
            {
                item.Disable();
                _pool.Enqueue(item);
            }
            
            _activeItems.Clear();
        }

        public T Get()
        {
            T item;
            
            if (_pool.Count > 0)
            {
                item = _pool.Dequeue();
                SetActive(item);
                
                return item;
            }
            
            item = _factoryItems.Create();
            SetActive(item);

            return item;
        }

        public void Release(T item)
        {
            if (item == null) 
                throw new ArgumentNullException(nameof(item));

            item.Disable();
            _pool.Enqueue(item);
            _activeItems.Remove(item);
            
            if (_activeItems.Count == 0)
                Completed?.Invoke();
        }

        public void SetActive(T item)
        {
            if (item == null) 
                throw new ArgumentNullException(nameof(item));
            
            item.Enable();
            _activeItems.Add(item);
            item.Init(this);
        }
    }
}