using System;
using System.Collections.Generic;
using Source.Scripts.Infrastructure.Factories.Interfaces;
using Source.Scripts.Infrastructure.Pools.Interfaces;

namespace Source.Scripts.Infrastructure.Pools
{
    public class Pool<T> : IPool<T> where T : IPoolable
    {
        private readonly IFactory<T> _factoryItems;
        private readonly int _startItemsCount;

        private Queue<T> _pool = new();
        private List<T> _activeItems = new();

        public Pool(IFactory<T> factoryItems, int startItemsCount)
        {
            _factoryItems = factoryItems ?? throw new ArgumentNullException(nameof(factoryItems));
            
            if (startItemsCount <= 0) 
                throw new ArgumentOutOfRangeException(nameof(startItemsCount));
            
            _startItemsCount = startItemsCount;
        }

        public int StartItemCount => _startItemsCount;
        public int AllItemsCount => _activeItems.Count + _pool.Count;
        public List<T> ActiveItems => _activeItems; // TODO: используется? нужно ли где-то?

        public void Init()
        {
            for (int i = 0; i < _startItemsCount; i++)
                Release(_factoryItems.Create());
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
        }

        public void SetActive(T item)
        {
            item.Enable();
            _activeItems.Add(item);
            item.Init(this);
        }
    }
}