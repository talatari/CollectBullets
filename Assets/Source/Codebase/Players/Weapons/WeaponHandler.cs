using System;
using System.Collections;
using Source.Codebase.Common;
using Source.Codebase.Players.PlayerModels;
using Source.Codebase.Players.Projectiles;
using UnityEngine;

namespace Source.Codebase.Players.Weapons
{
    public class WeaponHandler : MonoBehaviour
    {
        private const float RatioDecrement = 0.05f;
            
        [SerializeField] private ProjectilePlayer projectilePlayerPrefab;
        
        private Weapon _weapon;
        private Coroutine _shootingCoroutine;
        private Vector3 _direction;
        private CooldownTimer _cooldownTimer;
        private DamageStats _damageStats;
        private int _baseDelay;
        private bool _isRealoding => _cooldownTimer.IsFinished == false;
        private bool _isInit;

        public event Action Shoted;
        public event Action<int> Vampired; 

        public int ClipCapacity => _weapon.ClipCapacity;
        
        public void Init(DamageStats damageStats)
        {
            if (damageStats == null)
                throw new ArgumentNullException(nameof(damageStats));

            _damageStats = damageStats;

            _weapon = new Weapon(_damageStats.ClipCapacity);

            _baseDelay = _damageStats.ShootingDelay;
            _cooldownTimer = new CooldownTimer(_baseDelay);
            // TODO: create IFactory<ProjectileForPistol> -> pool
            // TODO: create pool projectile

            _damageStats.DamageChanged += projectilePlayerPrefab.SetDamage;
            _damageStats.BurningChanged += projectilePlayerPrefab.SetBurning;
            _damageStats.VampirismChanged += projectilePlayerPrefab.SetVampirism;
            _damageStats.ClipCapacityChanged += _weapon.SetClipCapacity;
            _damageStats.ShootingDelayChanged += OnShootingDelayChanged;

            _isInit = true;
        }

        private void Update()
        {
            if (_isInit == false)
                return;
            
            _cooldownTimer?.Tick(Time.deltaTime);
        }

        private void OnDisable()
        {
            if (_isInit == false)
                return;

            projectilePlayerPrefab.Vampired -= OnVampired;
            _damageStats.DamageChanged -= projectilePlayerPrefab.SetDamage;
            _damageStats.BurningChanged -= projectilePlayerPrefab.SetBurning;
            _damageStats.VampirismChanged -= projectilePlayerPrefab.SetVampirism;
            _damageStats.ClipCapacityChanged -= _weapon.SetClipCapacity;
            _damageStats.ShootingDelayChanged -= OnShootingDelayChanged;

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

        public void ResetCollectedBullets() => 
            _weapon.ResetCollectedBullets();

        public void StopShooting()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);
            
            _shootingCoroutine = null;
        }

        private void OnVampired(ProjectilePlayer projectilePlayer, int vampirism)
        {
            Vampired?.Invoke(vampirism);
            
            projectilePlayer.Vampired -= OnVampired;
        }

        private void OnShootingDelayChanged(int shootingDelay)
        {
            float newShootingDelay = _baseDelay - (shootingDelay - _baseDelay) * RatioDecrement;

            if (newShootingDelay < RatioDecrement)
                newShootingDelay = RatioDecrement;
            
            _cooldownTimer.SetCooldown(newShootingDelay);
        }

        private IEnumerator Shooting()
        {
            while (enabled)
            {
                if (_isRealoding)
                    yield return null;
                
                if (_weapon.CollectedBullets > 0 && _cooldownTimer.IsFinished)
                {
                    _weapon.Shoot();
                    Shoted?.Invoke();
                    _cooldownTimer.Run();
                    
                    ProjectilePlayer projectilePlayer = Instantiate(
                        projectilePlayerPrefab, new Vector3(
                            transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                    
                    projectilePlayer.Init(
                        _damageStats.Damage, 
                        _damageStats.Burning, 
                        _damageStats.BurningDuration, 
                        _damageStats.Vampirism);
                    projectilePlayer.Vampired += OnVampired;

                    // TODO: использовать пул 
                    // ProjectileForPistol projectileForPistol = _projectilePool.Get();
                    
                    // SetPositionAndRotation(new Vector3(
                    // transform.position.x, transform.position.y, transform.position.z), Quaternion.identity))'
                    
                    projectilePlayer.SetDirection(_direction);
                }
                
                yield return null;
            }
        }
    }
}