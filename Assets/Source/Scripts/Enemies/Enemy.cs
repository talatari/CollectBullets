using System;
using Source.Scripts.Enemies.Health;
using Source.Scripts.Infrastructure.Pools.Interfaces;
using Source.Scripts.SO;
using UnityEngine;
using IPoolable = Source.Scripts.Infrastructure.Pools.Interfaces.IPoolable;

namespace Source.Scripts.Enemies
{
    [RequireComponent(typeof(Mover))]
    public class Enemy : MonoBehaviour, IPoolable
    {
        [SerializeField] private EnemyScriptableObject _enemyScriptableObject;
        [SerializeField] private Mover _mover;
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private Attacker _attacker;
        
        private IPool<Enemy> _pool;
        private Transform _target;

        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            if (pool == null) 
                throw new ArgumentNullException(nameof(pool));
            
            _pool = pool as IPool<Enemy>;
            
            if (_pool == null)
                throw new ArgumentException("Pool must be of type IPool<Enemy>");
            
            _mover.SetSpeed(_enemyScriptableObject.Speed);
            _health.SetHealth(_enemyScriptableObject.Health);

            _attacker.Init(_enemyScriptableObject.Damage, _enemyScriptableObject.DistanceAttack, 
                _enemyScriptableObject.AttackCooldown);
        }

        private void OnEnable()
        {
            _health.EnemyDie += OnReleaseToPool;
            _attacker.PlayerDetected += OnMoveStop;
            _attacker.PlayerLost += OnMoveContinue;
        }

        private void OnDisable()
        {
            _health.EnemyDie -= OnReleaseToPool;
            _attacker.PlayerDetected -= OnMoveStop;
            _attacker.PlayerLost -= OnMoveContinue;
        }

        private void OnMoveStop() => 
            _mover.SetTarget(null);

        private void OnMoveContinue() => 
            _mover.SetTarget(_target);

        public void SetTarget(Transform target)
        {
            if (_mover == null)
                throw new ArgumentNullException(nameof(_mover));
            
            if (target == null) 
                throw new ArgumentNullException(nameof(target));

            _target = target;
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