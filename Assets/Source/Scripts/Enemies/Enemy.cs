using System;
using Source.Scripts.Infrastructure.Pools.Interfaces;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    [RequireComponent(typeof(Mover))]
    public class Enemy : MonoBehaviour, IPoolable
    {
        [SerializeField] private Mover _mover;
        
        private IPool<Enemy> _pool;

        public void SetTarget(Transform target)
        {
            if (_mover == null)
                throw new ArgumentNullException(nameof(_mover));
            
            if (target == null) 
                throw new ArgumentNullException(nameof(target));

            _mover.SetTarget(target);
        }

        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            if (pool == null) 
                throw new ArgumentNullException(nameof(pool));
            
            _pool = pool as IPool<Enemy>;
            
            if (_pool == null)
                throw new ArgumentException("Pool must be of type IPool<Enemy>");
        }

        public void Enable() => 
            gameObject.SetActive(true);

        public void Disable() => 
            gameObject.SetActive(false);

        public void ReleaseToPool() => 
            _pool.Release(this);
    }
}