using System;
using Source.Codebase.Behaviour;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Players.CollisionHandlers;
using Source.Codebase.Players.Joystick;
using Source.Codebase.Players.Movement;
using Source.Codebase.Players.PlayerModels;
using Source.Codebase.Players.Weapons;
using Source.Codebase.Upgrades;
using UnityEngine;

namespace Source.Codebase.Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Bag _bag;
        [SerializeField] private JoystickForRotator _joystickForRotator;
        [SerializeField] private Mover _mover;
        [SerializeField] private WeaponHandler _weaponHandler;
        [SerializeField] private Damageable _health;
        [SerializeField] private CollisionForBullets _collisionForBullets;
        [SerializeField] private CollisionForEnemies _collisionForEnemies;
        
        private Stats _stats;
        private UpgradeHandler _upgradeHandler;
        private GameLoopService _gameLoopService;
        private bool _isInit;

        public event Action Died;

        public void Init(Stats stats, UpgradeHandler upgradeHandler, GameLoopService gameLoopService)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
            _upgradeHandler = upgradeHandler ?? throw new ArgumentNullException(nameof(upgradeHandler));
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            
            _upgradeHandler.Init();
            
            _mover.Init(_stats.CommonStats.Speed);
            _weaponHandler.Init(_stats.DamageStats);
            _health.Init(_stats.HealthStats);
            _collisionForBullets.Init(_weaponHandler, _stats.CommonStats.Magnet);
            _collisionForEnemies.Init(this, _stats.CommonStats);
            _bag.CreateClip(_weaponHandler.ClipCapacity);

            _gameLoopService.WaveCompleted += OnWaveCompleted;
            _health.Died += OnDied;
            _stats.CommonStats.SpeedChanged += _mover.SetSpeed;
            _stats.CommonStats.MagnetChanged += _collisionForBullets.SetMagnet;
            _stats.CommonStats.FreezeChanged += _collisionForEnemies.SetFreeze;
            _stats.DamageStats.ClipCapacityChanged += _bag.CreateClip;
            _collisionForBullets.BulletCollected += OnCollected;
            _weaponHandler.Shoted += OnShoted;
            _weaponHandler.Vampired += OnVampire;

            _isInit = true;
        }

        private void OnDisable()
        {
            if (_isInit == false)
                return;
            
            _health.Died -= OnDied;
            _stats.CommonStats.SpeedChanged -= _mover.SetSpeed;
            _stats.CommonStats.MagnetChanged -= _collisionForBullets.SetMagnet;
            _stats.CommonStats.FreezeChanged -= _collisionForEnemies.SetFreeze;
            _stats.DamageStats.ClipCapacityChanged -= _bag.CreateClip;
            _collisionForBullets.BulletCollected -= OnCollected;
            _weaponHandler.Shoted -= OnShoted;
            _weaponHandler.Vampired -= OnVampire;
            _upgradeHandler.Dispose();
        }

        public void RotateToEnemy(Vector3 direction)
        {
            _joystickForRotator.SetEnemyPosition(direction);
            
            if (direction != Vector3.zero)
                _weaponHandler.StartShooting(direction);
        }
        
        public void StopShooting() => 
            _weaponHandler.StopShooting();
        
        public void TakeDamage(int damage)
        {
            if (damage <= 0)
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            _health.TakeDamage(damage);
        }
        
        public void FullRecovery()
        {
            print("FullRecovery._health.FullRecovery");
            
            _health.FullRecovery();
        }

        private void OnWaveCompleted()
        {
            print("OnWaveCompleted._health.FullRecovery");

            _health.FullRecovery();
        }

        private void OnDied() => 
            Died?.Invoke();

        private void OnVampire(int vampirism) => 
            _health.Heal(vampirism);
        
        private void OnCollected()
        {
            _weaponHandler.CollectBullet();
            _bag.CollectBullet(_weaponHandler.CollectedBullets);
        }
        
        private void OnShoted() => 
            _bag.UseCollectedBullets(_weaponHandler.CollectedBullets);
    }
}