using System;
using Source.Scripts.Behaviour;
using Source.Scripts.Players.CollisionHandlers;
using Source.Scripts.Players.Joystick;
using Source.Scripts.Players.Movement;
using Source.Scripts.Players.PlayerStats;
using Source.Scripts.Players.Weapons;
using UnityEngine;

namespace Source.Scripts.Players
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
        
        // TODO: создать класс Stats, в котором будут храниться базовые и измененные характеристики
        private const string AddCapacityName = "ADD CAPACITY";   // _weaponHandler._weapon._clipCapacity
        private const string BurningName = "BURNING";            // _weaponHandler._projectilePrefab._burning
        private const string DamageUpName = "DAMAGE UP";         // _weaponHandler._projectilePrefab._damage
        private const string FreezeName = "FREEZE";              // _collisionForEnemies._freeze
        private const string FullRecoveryName = "FULL RECOVERY"; // _health.FullRecovery();
        private const string MagnetName = "MAGNET";              // _collisionForBullets._radiusPickUpBullets
        private const string MaxHealthName = "MAX HEALTH";       // _health._maxHealth
        private const string RegenerationName = "REGENERATION";  // _health._regeneration
        private const string ShootingName = "SHOOTING";          // _weaponHandler._weapon._shootingDelay
        private const string SpeedUpName = "SPEED UP";           // _mover._speed
        private const string VampirismName = "VAMPIRISM";        // _weaponHandler._projectilePrefab._vampirism

        private Stats _stats;
        private bool _isInit;

        public void Init(Stats stats)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));

            _mover.Init(_stats.CommonStats.Speed);
            _weaponHandler.Init(_stats.DamageStats);
            _health.Init(_stats.HealthStats);
            _collisionForBullets.Init(_weaponHandler, _stats.CommonStats.Magnet);
            _collisionForEnemies.Init(this, _stats.CommonStats.Freeze);
            _bag.CreateClip(_weaponHandler.ClipCapacity);

            _stats.CommonStats.SpeedChanged += _mover.SetSpeed;
            _stats.CommonStats.MagnetChanged += _collisionForBullets.SetMagnet;
            _stats.CommonStats.FreezeChanged += _collisionForEnemies.SetFreeze;
            _stats.DamageStats.ClipCapacityChanged += _bag.CreateClip;
            _collisionForBullets.BulletCollected += OnCollected;
            _weaponHandler.Shoted += OnShoted;

            _isInit = true;
        }

        private void OnDisable()
        {
            if (_isInit == false)
                return;
            
            _stats.CommonStats.SpeedChanged -= _mover.SetSpeed;
            _stats.CommonStats.MagnetChanged -= _collisionForBullets.SetMagnet;
            _stats.CommonStats.FreezeChanged -= _collisionForEnemies.SetFreeze;
            _stats.DamageStats.ClipCapacityChanged -= _bag.CreateClip;
            _collisionForBullets.BulletCollected -= OnCollected;
            _weaponHandler.Shoted -= OnShoted;
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

        private void OnCollected()
        {
            _weaponHandler.CollectBullet();
            _bag.CollectBullet(_weaponHandler.CollectedBullets);
        }

        private void OnShoted() => 
            _bag.UseCollectedBullets(_weaponHandler.CollectedBullets);
    }
}