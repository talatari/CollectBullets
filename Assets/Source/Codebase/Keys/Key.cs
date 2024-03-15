using System;
using Source.Codebase.Infrastructure.Pools.Interfaces;
using UnityEngine;

namespace Source.Codebase.Keys
{
    public class Key : MonoBehaviour, IPoolable
    {
        [SerializeField] private KeyPointer _keyPointer;
        [SerializeField] private float _radius;
        
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

        public void OnReleaseToPool() => 
            _pool.Release(this);

        public void SetTarget(Transform player)
        {
            if (player == null) 
                throw new ArgumentNullException(nameof(player));
            if (_keyPointer == null)
                throw new ArgumentNullException(nameof(_keyPointer));
            
            _keyPointer.SetTarget(player);
            _keyPointer.SetRadius(_radius);
        }
    }
}