using System;
using System.Collections;
using Source.Scripts.Behaviour;
using Source.Scripts.Infrastructure.Pools.Interfaces;
using Source.Scripts.Players.PlayerModels;
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

            if (gameObject.activeSelf)
                StartCoroutine(Burning(damage, burningDuration));
        }

        public void Freeze(float freeze) => 
            _mover.Freeze(freeze);

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