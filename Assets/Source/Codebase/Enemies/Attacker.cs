using System;
using System.Collections;
using System.Linq;
using Source.Codebase.Common;
using Source.Codebase.Infrastructure.Factories;
using Source.Codebase.Infrastructure.Pools;
using Source.Codebase.Players;
using Source.Codebase.Players.CollisionHandlers;
using UnityEngine;

namespace Source.Codebase.Enemies
{
    public class Attacker : CollisionHandler
    {
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private ProjectileEnemy _projectilePrefab;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private Transform _container;
        
        private Collider[] _playerColliders = new Collider[MaxOverlap];
        private Pool<ProjectileEnemy> _poolProjectile;
        private CooldownTimer _cooldownTimer;
        private Coroutine _attackCoroutine;
        private int _radiusAttack;
        private int _attackCooldown;
        private bool _isRealoding;
        private int _damage;
        private int _startProjectileCount = 10;

        public event Action PlayerDetected;
        public event Action PlayerLost;

        public void Init(int damage, int distanceAttack, int attackCooldown)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            if (distanceAttack <= 0) 
                throw new ArgumentOutOfRangeException(nameof(distanceAttack));
            if (attackCooldown <= 0) 
                throw new ArgumentOutOfRangeException(nameof(attackCooldown));
            
            _damage = damage;
            _radiusAttack = distanceAttack / 2;
            _attackCooldown = attackCooldown;
        }

        private void Start()
        {
            _cooldownTimer = new CooldownTimer(_attackCooldown);
            
            if (_projectilePrefab == null)
                return;
            
            FactoryProjectile<ProjectileEnemy> factoryProjectile = new FactoryProjectile<ProjectileEnemy>(
                _projectilePrefab, _container);
            
            _poolProjectile = new Pool<ProjectileEnemy>(factoryProjectile, _startProjectileCount);
            _poolProjectile.Init();
        }

        public void Update()
        {
            OverlapPlayer();
            
            _cooldownTimer?.Tick(Time.deltaTime);
        }

        public void ReleaseAllProjectile() => 
            _poolProjectile?.ReleaseAll();

        private void OverlapPlayer()
        {
            int playerColliders = Physics.OverlapSphereNonAlloc(
                transform.position, _radiusAttack, _playerColliders, _playerLayer);

            if (playerColliders == 0)
            {
                StopShooting();
                PlayerLost?.Invoke();
                
                return;
            }

            if (_playerColliders.First(x => x != null).TryGetComponent(out Player player))
            {
                PlayerDetected?.Invoke();
                AttackPlayer(player);
            }
        }

        private void StopShooting()
        {
            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);
            
            _attackCoroutine = null;
        }

        private void AttackPlayer(Player player)
        {
            if (_attackCoroutine == null)
                _attackCoroutine = StartCoroutine(Attacking(player));
        }

        private IEnumerator Attacking(Player player)
        {
            while (enabled)
            {
                if (_cooldownTimer.IsFinished)
                {
                    if (_projectilePrefab == null)
                    {
                        player.TakeDamage(_damage);
                    }
                    else
                    {
                        ProjectileEnemy projectileEnemy = _poolProjectile.Get();
                        projectileEnemy.Init(_poolProjectile);
                        projectileEnemy.SetPosition(_attackPoint.position);
                        
                        Vector3 direction = player.transform.position - transform.position;
                        direction.y = 0;
                        
                        projectileEnemy.SetDirection(direction);
                    }
                    
                    _cooldownTimer.Run();
                }
                
                yield return null;
            }
        }
    }
}