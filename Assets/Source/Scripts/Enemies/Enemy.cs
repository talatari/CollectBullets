using System;
using System.Collections;
using Source.Scripts.Behaviour;
using Source.Scripts.Infrastructure.Pools.Interfaces;
using Source.Scripts.SO;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    [RequireComponent(typeof(Mover))]
    public class Enemy : MonoBehaviour, IPoolable
    {
        [SerializeField] private EnemyScriptableObject _enemyScriptableObject;
        [SerializeField] private Mover _mover;
        [SerializeField] private Damageable _health;
        [SerializeField] private Attacker _attacker;
        [SerializeField] private EnemyPointer _enemyPointer;
        
        private IPool<Enemy> _pool;
        private Transform _target;
        private int _burningDelay = 1;

        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            if (pool == null) 
                throw new ArgumentNullException(nameof(pool));
            
            _pool = pool as IPool<Enemy>;
            
            if (_pool == null)
                throw new ArgumentException("Pool must be of type IPool<Enemy>");
            
            _mover.SetSpeed(_enemyScriptableObject.Speed);
            
            _attacker.Init(
                _enemyScriptableObject.Damage,
                _enemyScriptableObject.DistanceAttack, 
                _enemyScriptableObject.AttackCooldown);
            
            _health.Init(_enemyScriptableObject.MaxHealth);
        }

        public event Action<Enemy> Died;
        
        private void OnEnable()
        {
            _health.Died += OnReleaseToPool;
            _attacker.PlayerDetected += OnMoveStop;
            _attacker.PlayerLost += OnMoveContinue;
        }

        private void OnDisable()
        {
            _health.Died -= OnReleaseToPool;
            _attacker.PlayerDetected -= OnMoveStop;
            _attacker.PlayerLost -= OnMoveContinue;
        }

        public void SetTarget(Transform target)
        {
            if (_mover == null)
                throw new ArgumentNullException(nameof(_mover));
            
            if (target == null) 
                throw new ArgumentNullException(nameof(target));
            
            if (_enemyPointer == null)
                throw new ArgumentNullException(nameof(_enemyPointer));

            _target = target;
            _mover.SetTarget(target);
            _enemyPointer.SetTarget(_target);
        }

        public void Enable() => 
            gameObject.SetActive(true);

        public void Disable() => 
            gameObject.SetActive(false);

        public void OnReleaseToPool()
        {
            Died?.Invoke(this);
            _pool.Release(this);
        }

        public void TakeDamage(int damage)
        {
            if (_health == null)
                return;
            
            _health.TakeDamage(damage);
        }

        public void Burn(int damage, int burningDuration)
        {
            if (damage <= 0)
                throw new ArgumentOutOfRangeException(nameof(damage));
            if (burningDuration <= 0)
                throw new ArgumentOutOfRangeException(nameof(burningDuration));
            
            StartCoroutine(Burning(damage, burningDuration));
        }

        public void Freeze(float freeze) => 
            _mover.Freeze(freeze);

        private void OnMoveStop() => 
            _mover.SetTarget(null);

        private void OnMoveContinue() => 
            _mover.SetTarget(_target);

        private IEnumerator Burning(int damage, int burningDuration)
        {
            WaitForSeconds burningDelay = new WaitForSeconds(_burningDelay);
            
            for (int i = 0; i < burningDuration; i++)
            {
                _health.TakeDamage(damage);
                
                yield return burningDelay;
            }
        }
    }
}