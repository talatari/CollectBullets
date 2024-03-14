using System;
using Source.Codebase.Infrastructure.Pools.Interfaces;
using UnityEngine;

namespace Source.Codebase.Keys
{
    public class Key : MonoBehaviour, IPoolable
    {
        private IPool<Key> _pool;

        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            if (pool == null) 
                throw new ArgumentNullException(nameof(pool));
            
            _pool = pool as IPool<Key>;
            
            if (_pool == null)
                throw new ArgumentException("Pool must be of type IPool<Key>");
        }

        public void Enable() => 
            gameObject.SetActive(true);

        public void Disable() => 
            gameObject.SetActive(false);

        public void OnReleaseToPool()
        {
            _pool.Release(this);
            _pool.SetKeyCollected();
        }
    }
}