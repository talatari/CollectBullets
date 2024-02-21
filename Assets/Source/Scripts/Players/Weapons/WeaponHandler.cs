using System;
using System.Collections;
using Source.Scripts.Common;
using Source.Scripts.Players.PlayerModels;
using Source.Scripts.Players.Projectiles;
using UnityEngine;

namespace Source.Scripts.Players.Weapons
{
    public class WeaponHandler : MonoBehaviour
    {
        private const float RatioDecrement = 0.05f;
        private const int BaseDelay = 1;
            
        [SerializeField] private ProjectileForPistol _projectilePrefab;
        
        private Weapon _weapon;
        private Coroutine _shootingCoroutine;
        private Vector3 _direction;
        private bool _isRealoding;
        private CooldownTimer _cooldownTimer;
        private DamageStats _damageStats;
        private int _collectedBullets;
        private bool _isInit;

        public event Action Shoted;

        public int ClipCapacity => _weapon.ClipCapacity;
        public int CollectedBullets => _collectedBullets;

        public void Init(DamageStats damageStats)
        {
            if (damageStats == null)
                throw new ArgumentNullException(nameof(damageStats));

            _damageStats = damageStats;

            _weapon = new Weapon(_damageStats.ClipCapacity);
            _cooldownTimer = new CooldownTimer(_damageStats.ShootingDelay);
            // TODO: create IFactory<ProjectileForPistol> -> pool
            // TODO: create pool projectile
            
            _damageStats.DamageChanged += _projectilePrefab.SetDamage;
            _damageStats.BurningChanged += _projectilePrefab.SetBurning;
            _damageStats.VampirismChanged += _projectilePrefab.SetVampirism;
            _damageStats.ClipCapacityChanged += _weapon.SetClipCapacity;
            _damageStats.ShootingDelayChanged += OnShootingDelayChanged;
            
            _weapon.CollectedBulletsChanged += OnCollectedBulletsChanged;

            _isInit = true;
        }
        
        private void Update()
        {
            if (_isInit == false)
                return;
            
            _cooldownTimer.Tick(Time.deltaTime);
        }

        private void OnDisable()
        {
            if (_isInit == false)
                return;

            _damageStats.DamageChanged -= _projectilePrefab.SetDamage;
            _damageStats.BurningChanged -= _projectilePrefab.SetBurning;
            _damageStats.VampirismChanged -= _projectilePrefab.SetVampirism;
            _damageStats.ClipCapacityChanged -= _weapon.SetClipCapacity;
            _damageStats.ShootingDelayChanged -= OnShootingDelayChanged;
            
            _weapon.CollectedBulletsChanged -= OnCollectedBulletsChanged;

            StopShooting();
        }

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

        private void OnShootingDelayChanged(int shootingDelay)
        {
            float newShootingDelay = BaseDelay - (shootingDelay - BaseDelay) * RatioDecrement;
            
            _cooldownTimer.SetCooldown(newShootingDelay);
        }

        private void OnCollectedBulletsChanged(int collectedBullets) => 
            _collectedBullets = collectedBullets;

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
                    
                    projectileForPistol.Init(_damageStats.Damage, _damageStats.Burning, _damageStats.Vampirism);

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