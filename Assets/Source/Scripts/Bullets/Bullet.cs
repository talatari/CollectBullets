using System;
using Source.Scripts.Infrastructure.Pools.Interfaces;
using UnityEngine;

namespace Source.Scripts.Bullets
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        private IPool<Bullet> _pool;

        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            if (pool == null) 
                throw new ArgumentNullException(nameof(pool));
            
            _pool = pool as IPool<Bullet>;
            
            if (_pool == null)
                throw new ArgumentException("Pool must be of type IPool<Bullet>");
        }

        public void Enable() => 
            gameObject.SetActive(true);

        public void Disable() => 
            gameObject.SetActive(false);

        public void ReleaseToPool() => 
            _pool.Release(this);
    }
}