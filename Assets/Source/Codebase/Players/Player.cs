using System;
using Source.Codebase.Behaviour;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Players.Bug;
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
        [SerializeField] private CollisionForKeys _collisionForKeys;
        
        private Stats _stats;
        private UpgradeHandler _upgradeHandler;
        private GameLoopMediator _gameLoopMediator;
        private bool _isInit;

        public event Action Died;

        public void Init(Stats stats, UpgradeHandler upgradeHandler, GameLoopMediator gameLoopMediator)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
            _upgradeHandler = upgradeHandler ?? throw new ArgumentNullException(nameof(upgradeHandler));
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));
            
            _upgradeHandler.Init();
            
            _mover.Init(_stats.CommonStats.Speed);
            _weaponHandler.Init(_stats.DamageStats);
            _health.Init(_stats.HealthStats);
            _collisionForBullets.Init(_bag, _stats.CommonStats.Magnet);
            _collisionForEnemies.Init(this, _stats.CommonStats);
            _collisionForKeys.Init(_bag, _stats.CommonStats.Magnet);
            _bag.CreateClip(_weaponHandler.ClipCapacity);

            _gameLoopMediator.WaveCompleted += OnWaveCompleted;
            _health.Died += OnDied;
            _stats.CommonStats.SpeedChanged += _mover.SetSpeed;
            _stats.CommonStats.MagnetChanged += OnSetMagnet;
            _stats.CommonStats.FreezeChanged += _collisionForEnemies.SetFreeze;
            _stats.DamageStats.ClipCapacityChanged += _bag.AddSlot;
            _collisionForBullets.BulletCollected += OnBulletCollected;
            _collisionForKeys.KeyCollected += OnKeyCollected;
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
            _stats.CommonStats.MagnetChanged -= OnSetMagnet;
            _stats.CommonStats.FreezeChanged -= _collisionForEnemies.SetFreeze;
            _stats.DamageStats.ClipCapacityChanged -= _bag.AddSlot;
            _collisionForBullets.BulletCollected -= OnBulletCollected;
            _collisionForKeys.KeyCollected -= OnKeyCollected;
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

        public void Restart()
        {
            _health.FullRecovery();
            _weaponHandler.ResetCollectedBullets();
            _bag.Reset();
            _upgradeHandler.SetDefaultValues();
        }

        public bool HaveCollectedKey() =>
            _collisionForKeys.IsKeyCollected;

        public void UseKey()
        {
            _bag.UseKey();
            _collisionForKeys.UseKey();
        }

        private void OnWaveCompleted(int numberWave) => 
            _health.FullRecovery();

        private void OnDied() => 
            Died?.Invoke();

        private void OnVampire(int vampirism) => 
            _health.Heal(vampirism);

        private void OnShoted() => 
            _bag.UseCollectedBullet();

        private void OnSetMagnet(int magnet)
        {
            _collisionForBullets.SetRadiusPickUp(magnet);
            _collisionForKeys.SetRadiusPickUp(magnet);
        }

        private void OnBulletCollected()
        {
            _weaponHandler.CollectBullet();
            _bag.CollectBullet();
        }

        private void OnKeyCollected()
        {
            _bag.CollecteKey();
            _gameLoopMediator.NotifyKeyCollected();
        }
    }
}