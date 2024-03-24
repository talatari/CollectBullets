using System;
using System.Linq;
using Source.Codebase.Common;
using Source.Codebase.Infrastructure.Pools.Interfaces;
using Source.Codebase.Players;
using Source.Codebase.Players.CollisionHandlers;
using Source.Codebase.SO;
using UnityEngine;

namespace Source.Codebase.Enemies
{
    public class ProjectileEnemy : CollisionHandler, IPoolable
    {
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private ProjectileScriptableObject _projectileScriptableObject;

        private Collider[] _playerColliders = new Collider[MaxOverlap];
        private IPool<ProjectileEnemy> _poolProjectileEnemy;
        private CooldownTimer _cooldownTimer;
        private Vector3 _direction;
        private float _speed;
        private float _radius;
        private int _damage;
        private bool _isInit;

        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            if (pool == null) 
                throw new ArgumentNullException(nameof(pool));
            
            _poolProjectileEnemy = pool as IPool<ProjectileEnemy>;
            
            if (_poolProjectileEnemy == null)
                throw new ArgumentException("Pool must be of type IPool<ProjectileEnemy>");
            
            _damage = _projectileScriptableObject.Damage;
            _speed = _projectileScriptableObject.Speed;
            
            float _diameter = transform.localScale.x;
            _radius = _diameter / 2;
            
            _cooldownTimer = new CooldownTimer(_projectileScriptableObject.LifeTime);
            _cooldownTimer.Run();
            
            _isInit = true;
        }

        private void Update()
        {
            if (_isInit == false)
                return;
            
            if (_cooldownTimer == null)
                return;
            
            OverlapEnemies();
            
            Move();
            
            _cooldownTimer.Tick(Time.deltaTime);
            
            if (_cooldownTimer.IsFinished)
                OnReleaseToPool();
        }

        public void Enable()
        {
            transform.position = Vector3.zero;
            gameObject.SetActive(true);
        }

        public void Disable() => 
            gameObject.SetActive(false);

        public void OnReleaseToPool() => 
            _poolProjectileEnemy.Release(this);

        public void SetPosition(Vector3 attackPointPosition) => 
            transform.position = attackPointPosition;

        public void SetDirection(Vector3 direction) => 
            _direction = direction;

        private void OverlapEnemies()
        {
            int playerColliders = Physics.OverlapSphereNonAlloc(
                transform.position, _radius, _playerColliders, _playerLayer);
            
            if (playerColliders == 0)
                return;
            
            if (_playerColliders.First(playerCollider => playerCollider != null).TryGetComponent(out Player player))
            {
                player.TakeDamage(_damage);
                OnReleaseToPool();
            }
        }

        private void Move() => 
            transform.Translate(_direction * (_speed * Time.deltaTime));
    }
}