using System;
using System.Collections;
using Source.Scripts.Common;
using Source.Scripts.Players.PlayerStats;
using Source.Scripts.Players.Projectiles;
using UnityEngine;

namespace Source.Scripts.Players.Weapons
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private ProjectileForPistol _projectilePrefab;
        
        private Weapon _weapon;
        private Coroutine _shootingCoroutine;
        private Vector3 _direction;
        private bool _isRealoding;
        private CooldownTimer _cooldownTimer;
        private DamageStats _damageStats;

        public event Action Shoted;

        public int ClipCapacityBullets => _weapon.ClipCapacityBullets;
        public int CollectedBullets => _weapon.CollectedBullets;

        public void Init(DamageStats damageStats)
        {
            if (damageStats == null)
                throw new ArgumentNullException(nameof(damageStats));

            _damageStats = damageStats;
            _projectilePrefab.Init(_damageStats.Damage);
        }

        private void Awake()
        {
            _weapon = new Weapon(_damageStats);
            _cooldownTimer = new CooldownTimer(_damageStats.ShootingDelay);
            
            // TODO: create IFactory<ProjectileForPistol> -> pool
            // TODO: create pool projectile
        }

        private void Update() => 
            _cooldownTimer.Tick(Time.deltaTime);

        private void OnDisable() => 
            StopShooting();

        public void StartShooting(Vector3 direction)
        {
            if (_isRealoding)
                return;
            
            _direction = direction;
            
            if (_shootingCoroutine == null)
                _shootingCoroutine = StartCoroutine(Shooting());
        }

        public void CollectBullet() => 
            _weapon.CollectBullet();

        public void StopShooting()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);
            
            _shootingCoroutine = null;
        }

        private IEnumerator Shooting()
        {
            while (enabled)
            {
                if (_weapon.CollectedBullets > 0 && _cooldownTimer.CanShoot)
                {
                    _weapon.Shoot();
                    Shoted?.Invoke();
                    _cooldownTimer.Run();
                    
                    ProjectileForPistol projectileForPistol = Instantiate(
                        _projectilePrefab, new Vector3(
                            transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                    
                    // TODO: использовать пул 
                    // ProjectileForPistol projectileForPistol = _projectilePool.Get();
                    
                    // SetPositionAndRotation(new Vector3(
                    // transform.position.x, transform.position.y, transform.position.z), Quaternion.identity))'
                    
                    projectileForPistol.SetDirection(_direction);
                }
                
                yield return null;
            }
        }
    }
}