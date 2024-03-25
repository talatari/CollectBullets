using System;
using System.Collections;
using Source.Codebase.Common;
using Source.Codebase.Infrastructure.Factories;
using Source.Codebase.Infrastructure.Pools;
using Source.Codebase.Players.PlayerModels;
using Source.Codebase.Players.Projectiles;
using UnityEngine;

namespace Source.Codebase.Players.Weapons
{
    public class WeaponHandler : MonoBehaviour
    {
        private const float RatioDecrement = 0.05f;
            
        [SerializeField] private ProjectilePlayer _projectilePrefab;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private Transform _container;
        
        private Pool<ProjectilePlayer> _poolProjectile;
        private Weapon _weapon;
        private Coroutine _shootingCoroutine;
        private Vector3 _direction;
        private CooldownTimer _cooldownTimer;
        private DamageStats _damageStats;
        private int _baseDelay;
        private int _startProjectileCount = 10;
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

            _damageStats.DamageChanged += _projectilePrefab.SetDamage;
            _damageStats.BurningChanged += _projectilePrefab.SetBurning;
            _damageStats.VampirismChanged += _projectilePrefab.SetVampirism;
            _damageStats.ClipCapacityChanged += _weapon.SetClipCapacity;
            _damageStats.ShootingDelayChanged += OnShootingDelayChanged;

            FactoryProjectile<ProjectilePlayer> factoryProjectile = new FactoryProjectile<ProjectilePlayer>(
                _projectilePrefab, _container);
            
            _poolProjectile = new Pool<ProjectilePlayer>(factoryProjectile, _startProjectileCount);
            _poolProjectile.Init();
            
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

            _projectilePrefab.Vampired -= OnVampired;
            _damageStats.DamageChanged -= _projectilePrefab.SetDamage;
            _damageStats.BurningChanged -= _projectilePrefab.SetBurning;
            _damageStats.VampirismChanged -= _projectilePrefab.SetVampirism;
            _damageStats.ClipCapacityChanged -= _weapon.SetClipCapacity;
            _damageStats.ShootingDelayChanged -= OnShootingDelayChanged;

            StopShooting();
        }

        public void StartShooting(Vector3 direction)
        {
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
                if (_weapon.CollectedBullets > 0 && _cooldownTimer.IsFinished)
                {
                    _weapon.Shoot();
                    Shoted?.Invoke();
                    _cooldownTimer.Run();
                    
                    ProjectilePlayer projectilePlayer = _poolProjectile.Get();
                    
                    projectilePlayer.Init(_poolProjectile);
                    
                    projectilePlayer.InitStats(
                        _damageStats.Damage, 
                        _damageStats.Burning, 
                        _damageStats.BurningDuration, 
                        _damageStats.Vampirism);
                    projectilePlayer.Vampired += OnVampired;
                    
                    projectilePlayer.SetPosition(_attackPoint.position);
                    
                    projectilePlayer.SetDirection(_direction);
                }
                
                yield return null;
            }
        }
    }
}