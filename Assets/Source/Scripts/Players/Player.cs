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
    [RequireComponent(typeof(CollisionForBullets))]
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
        private const string AddCapacityName = "ADD CAPACITY";   // _weaponHandler._weapon.ClipCapacity
        private const string BurningName = "BURNING";            // TODO: Burning - нужно реализовать таймер у врага который будет дамажить 1-4 сек
        private const string DamageUpName = "DAMAGE UP";         // _weaponHandler._projectilePrefab.Damage
        private const string FreezeName = "FREEZE";              // TODO: Freeze - нужно менять статы врагов
        private const string FullRecoveryName = "FULL RECOVERY"; // _health.FullRecovery();
        private const string MagnetName = "MAGNET";              // _collisionForBullets._radiusPickUpBullets
        private const string MaxHealthName = "MAX HEALTH";       // _health._maxHealth
        private const string RegenerationName = "REGENERATION";  // _health._regeneration
        private const string ShootingName = "SHOOTING";          // _weaponHandler._weapon._shootingDelay
        private const string SpeedUpName = "SPEED UP";           // _mover._speed
        private const string VampirismName = "VAMPIRISM";        // TODO: Vampirism - пока что не реализован

        private Stats _stats;

        public void Init(Stats stats)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));

            _mover.Init(_stats.CommonStats);
            _weaponHandler.Init(_stats.DamageStats);
            _health.Init(_stats.HealthStats);
            _collisionForBullets.Init(_weaponHandler, _stats.CommonStats);
            _collisionForEnemies.Init(this);
        }

        private void Start() => 
            _bag.CreateClip(_weaponHandler.ClipCapacity);

        private void OnEnable()
        {
            _collisionForBullets.BulletCollected += OnCollected;
            _weaponHandler.Shoted += OnShoted;
        }

        private void OnDisable()
        {
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