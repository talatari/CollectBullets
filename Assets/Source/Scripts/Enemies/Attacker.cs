using System;
using System.Collections;
using System.Linq;
using Source.Scripts.Common;
using Source.Scripts.Players;
using Source.Scripts.Players.CollisionHandlers;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    public class Attacker : CollisionHandler
    {
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private ProjectileEnemy _projectilePrefab;
        [SerializeField] private Transform _attackPoint;

        private Collider[] _playerColliders = new Collider[MaxOverlap];
        private CooldownTimer _cooldownTimer;
        private Coroutine _attackCoroutine;
        private float _distanceAttack;
        private float _attackCooldown;
        private bool _isRealoding;
        private int _damage;

        public event Action PlayerDetected;
        public event Action PlayerLost;

        public void Init(int damage, float distanceAttack, float attackCooldown)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            if (distanceAttack < 0)
                distanceAttack = 0;
            if (attackCooldown <= 0) 
                throw new ArgumentOutOfRangeException(nameof(attackCooldown));
            
            _damage = damage;
            _distanceAttack = distanceAttack; // TODO: implement random range for distance
            _attackCooldown = attackCooldown;
        }

        private void Start() => 
            _cooldownTimer = new CooldownTimer(_attackCooldown);

        public void Update()
        {
            OverlapPlayer();
            
            _cooldownTimer.Tick(Time.deltaTime);
        }

        private void OverlapPlayer()
        {
            int playerColliders = Physics.OverlapSphereNonAlloc(
                transform.position, _distanceAttack, _playerColliders, _playerLayer);

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
                if (_cooldownTimer.CanShoot)
                {
                    if (_projectilePrefab == null)
                        player.TakeDamage(_damage);
                    else
                        CreateProjectile(player);
                    
                    _cooldownTimer.Run();
                }

                yield return null;
            }
        }

        private void CreateProjectile(Player player)
        {
            ProjectileEnemy projectileEnemy = Instantiate(
                _projectilePrefab, _attackPoint.transform.position, Quaternion.identity);
                
            Vector3 rotateDirection = player.transform.position - transform.position;
            rotateDirection.y = 0;
            projectileEnemy.SetDirection(rotateDirection);
        }
    }
}