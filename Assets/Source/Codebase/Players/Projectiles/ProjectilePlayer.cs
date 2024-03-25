using System;
using System.Linq;
using Source.Codebase.Common;
using Source.Codebase.Enemies;
using Source.Codebase.Infrastructure.Pools.Interfaces;
using Source.Codebase.Players.CollisionHandlers;
using Source.Codebase.SO;
using UnityEngine;

namespace Source.Codebase.Players.Projectiles
{
    public class ProjectilePlayer : CollisionHandler, IPoolable
    {
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private ProjectileScriptableObject _projectileConfig;
        
        private Collider[] _enemyColliders = new Collider[MaxOverlap];
        private IPool<ProjectilePlayer> _poolProjectilePlayer;
        private CooldownTimer _cooldownTimer;
        private Vector3 _direction;
        private int _damage;
        private int _burning;
        private int _burningDuration;
        private int _vampirism;
        private int _speed;
        private float _radius;
        private bool _isInit;

        public event Action<ProjectilePlayer, int> Vampired;

        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            if (pool == null) 
                throw new ArgumentNullException(nameof(pool));
            
            _poolProjectilePlayer = pool as IPool<ProjectilePlayer>;
            
            if (_poolProjectilePlayer == null)
                throw new ArgumentException("Pool must be of type IPool<ProjectilePlayer>");
        }

        public void InitStats(int damage, int burning, int burningDuration, int vampirism)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            if (burning < 0)
                throw new ArgumentOutOfRangeException(nameof(burning));
            if (burningDuration < 0)
                throw new ArgumentOutOfRangeException(nameof(burningDuration));
            if (vampirism < 0) 
                throw new ArgumentOutOfRangeException(nameof(vampirism));

            _damage = damage;
            _burning = burning;
            _burningDuration = burningDuration;
            _vampirism = vampirism;
            
            _speed = _projectileConfig.Speed;

            float _diameter = transform.localScale.x;
            _radius = _diameter / 2;
            
            _cooldownTimer = new CooldownTimer(_projectileConfig.LifeTime);
            _cooldownTimer.Run();
            
            _isInit = true;
            
        }

        public void Enable()
        {
            transform.position = Vector3.zero;
            gameObject.SetActive(true);
        }

        public void Disable() => 
            gameObject.SetActive(false);

        public void OnReleaseToPool() => 
            _poolProjectilePlayer.Release(this);

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

        public void SetDamage(int value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _damage = value;
        }

        public void SetBurning(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _burning = value;
        }

        public void SetVampirism(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _vampirism = value;
        }

        public void SetPosition(Vector3 attackPointPosition) => 
            transform.position = attackPointPosition;

        public void SetDirection(Vector3 direction) => 
            _direction = direction;

        private void OverlapEnemies()
        {
            int enemiesAmount = Physics.OverlapSphereNonAlloc(
                transform.position, _radius, _enemyColliders, _enemyLayer);
            
            if (enemiesAmount == 0)
                return;
            
            if (_enemyColliders.First(enemyCollider => enemyCollider != null).TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(_damage);
                
                if (_burning > 0)
                    enemy.Burn(_burning, _burningDuration);

                if (_vampirism > 0)
                    Vampired?.Invoke(this, _vampirism);
                
                OnReleaseToPool();
            }
        }

        private void Move() => 
            transform.Translate(_direction * (_speed * Time.deltaTime));
    }
}