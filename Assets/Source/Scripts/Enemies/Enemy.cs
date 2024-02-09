using System;
using Source.Scripts.Enemies.SO;
using Source.Scripts.Infrastructure.Pools.Interfaces;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    [RequireComponent(typeof(Mover))]
    public class Enemy : MonoBehaviour, IPoolable
    {
        [SerializeField] private EnemyScriptableObject _enemyScriptableObject;
        [SerializeField] private Mover _mover;
        [SerializeField] private Health _health;
        [SerializeField] private Attacker _attacker;
        [SerializeField] private Collider _collider;
        
        private IPool<Enemy> _pool;

        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            if (pool == null) 
                throw new ArgumentNullException(nameof(pool));
            
            _pool = pool as IPool<Enemy>;
            
            if (_pool == null)
                throw new ArgumentException("Pool must be of type IPool<Enemy>");
            
            _mover.SetSpeed(_enemyScriptableObject.Speed);
            _health.SetHealth(_enemyScriptableObject.Health);
            _attacker.SetDamage(_enemyScriptableObject.Damage);
        }

        private void OnEnable() => 
            _health.EnemyDie += OnReleaseToPool;

        private void OnDisable() => 
            _health.EnemyDie -= OnReleaseToPool;

        public void SetTarget(Transform target)
        {
            if (_mover == null)
                throw new ArgumentNullException(nameof(_mover));
            
            if (target == null) 
                throw new ArgumentNullException(nameof(target));

            _mover.SetTarget(target);
        }

        public void Enable() => 
            gameObject.SetActive(true);

        public void Disable() => 
            gameObject.SetActive(false);

        public void OnReleaseToPool() => 
            _pool.Release(this);

        public void TakeDamage(int damage)
        {
            if (_health == null)
                return;
            
            _health.TakeDamage(damage);
        }
    }
}