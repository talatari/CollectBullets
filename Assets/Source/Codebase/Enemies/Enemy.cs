using System;
using System.Collections;
using Source.Codebase.Behaviour;
using Source.Codebase.Infrastructure.Pools.Interfaces;
using Source.Codebase.Players.PlayerModels;
using Source.Codebase.SO;
using UnityEngine;
using UnityEngine.AI;

namespace Source.Codebase.Enemies
{
    [RequireComponent(typeof(Mover))]
    public class Enemy : MonoBehaviour, IPoolable
    {
        [SerializeField] private EnemyScriptableObject _config;
        [SerializeField] private Damageable _health;
        [SerializeField] private Mover _mover;
        [SerializeField] private Attacker _attacker;
        [SerializeField] private EnemyPointer _enemyPointer;
        [SerializeField] private NavMeshAgent _agent;
        
        private IPool<Enemy> _pool;
        private Transform _player;
        private int _burningDelay = 1;
        private CommonStats _commonStats;

        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            if (pool == null) 
                throw new ArgumentNullException(nameof(pool));
            
            _pool = pool as IPool<Enemy>;
            
            if (_pool == null)
                throw new ArgumentException("Pool must be of type IPool<Enemy>");

            _health.Init(_config.MaxHealth);
            _mover.Init(_config.Speed, _config.DistanceAttack);
            _attacker.Init(_config.Damage, _config.DistanceAttack, _config.AttackCooldown);
        }

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

        private void OnDestroy() => 
            _commonStats.RadiusAttackChanged -= OnSetRadius;

        public void SetTarget(Transform player, CommonStats commonStats)
        {
            if (player == null) 
                throw new ArgumentNullException(nameof(player));
            if (commonStats == null)
                throw new ArgumentNullException(nameof(commonStats));
            if (_mover == null)
                throw new ArgumentNullException(nameof(_mover));
            if (_enemyPointer == null)
                throw new ArgumentNullException(nameof(_enemyPointer));
            
            _player = player;
            _commonStats = commonStats;
            _mover.SetTarget(_player);
            _enemyPointer.SetTarget(_player);
            _enemyPointer.SetRadius(_commonStats.RadiusAttack);
            _commonStats.RadiusAttackChanged += OnSetRadius;
        }

        public void Enable() => 
            gameObject.SetActive(true);

        public void Disable() => 
            gameObject.SetActive(false);

        public void OnReleaseToPool()
        {
            StopAllCoroutines();
            _pool.Release(this);
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            
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

            if (gameObject.activeSelf)
                StartCoroutine(Burning(damage, burningDuration));
        }

        public void Freeze(float freeze)
        {
            if (freeze < 0)
                throw new ArgumentOutOfRangeException(nameof(freeze));
            
            _mover.Freeze(freeze);
        }

        public void NavMeshAgentActive() => 
            _agent.enabled = true;

        private void OnMoveStop() => 
            _mover.SetTarget(null);

        private void OnMoveContinue() => 
            _mover.SetTarget(_player);

        private void OnSetRadius(int radiusAttack) => 
            _enemyPointer.SetRadius(radiusAttack);

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