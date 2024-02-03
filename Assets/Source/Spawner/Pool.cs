using System;
using System.Collections.Generic;

namespace Source.Spawner
{
    public class Pool<T>
    {
        private readonly Func<T> _preloadFunc;
        private readonly Action<T> _getAction;
        private readonly Action<T> _returnAction;
        private readonly int _preloadCount;
        private Queue<T> _pool = new ();
        private List<T> _activeItems = new ();

        public Pool(Func<T> preloadFunc, Action<T> getAction, Action<T> returnAction, int preloadCount)
        {
            _preloadFunc = preloadFunc ?? throw new ArgumentNullException(nameof(preloadFunc));
            _getAction = getAction ?? throw new ArgumentNullException(nameof(getAction));
            _returnAction = returnAction ?? throw new ArgumentNullException(nameof(returnAction));
            
            if (preloadCount <= 0) 
                throw new ArgumentOutOfRangeException(nameof(preloadCount));
            
            _preloadCount = preloadCount;

            Spawn();
        }

        public T Get()
        {
            T item = _pool.Count > 0 ? _pool.Dequeue() : _preloadFunc();
            _getAction(item);
            _activeItems.Add(item);

            return item;
        }

        public void Return(T item)
        {
            if (item == null) 
                throw new ArgumentNullException(nameof(item));
            
            _returnAction(item);
            _pool.Enqueue(item);
            _activeItems.Remove(item);
        }

        private void Spawn()
        {
            for (int i = 0; i < _preloadCount; i++)
                Return(_preloadFunc());
        }

        public void ReturnAll()
        {
            foreach (T item in _activeItems.ToArray())
                Return(item);
        }
    }
}