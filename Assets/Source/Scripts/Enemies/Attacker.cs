using System;
using System.Collections;
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
        
        private int _damage;
        private float _distanceAttack;
        private Collider[] _playerCollider = new Collider[MaxOverlap];
        private CooldownTimer _cooldownTimer;
        private float _attackCooldown;
        private bool _isRealoding;
        private Coroutine _attackCoroutine;

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
            _distanceAttack = distanceAttack;
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
                transform.position, _distanceAttack, _playerCollider, _playerLayer);

            if (playerColliders == 0)
            {
                StopShooting();
                PlayerLost?.Invoke();
                
                return;
            }

            for (int i = 0; i < playerColliders; i++)
                if (_playerCollider[i].TryGetComponent(out Player player))
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
                _attackCoroutine = StartCoroutine(Shooting(player));
        }

        private IEnumerator Shooting(Player player)
        {
            while (enabled)
            {
                if (_cooldownTimer.CanShoot == false)
                    yield break;
                
                if (_projectilePrefab != null)
                {
                    ProjectileEnemy projectileEnemy = Instantiate(
                        _projectilePrefab, _attackPoint.transform.position, Quaternion.identity);
                
                    Vector3 rotateDirection = player.transform.position - transform.position;
                    rotateDirection.y = 0;
                    projectileEnemy.SetDirection(rotateDirection);
                }
                
                player.TakeDamage(_damage);
                _cooldownTimer.Run();
                
                yield return new WaitForSeconds(_attackCooldown);
            }
        }
    }
}