using System;
using Source.Scripts.Behaviour;
using Source.Scripts.Players.CollisionHandlers;
using Source.Scripts.Players.Joystick;
using Source.Scripts.Players.Movement;
using Source.Scripts.Players.PlayerStats;
using Source.Scripts.Players.Weapons;
using Source.Scripts.SO;
using UnityEngine;

namespace Source.Scripts.Players
{
    [RequireComponent(typeof(CollisionForBullets), typeof(Mover))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Bag _bag;
        [SerializeField] private JoystickForRotator _joystickForRotator;
        [SerializeField] private WeaponHandler _weaponHandler;
        [SerializeField] private Damageable _health;
        
        // TODO: создать класс Stats, в котором будут храниться базовые и измененные характеристики
        private const string AddCapacityName = "ADD CAPACITY";   // _weaponHandler._weapon.ClipCapacity
        private const string BurningName = "BURNING";            // TODO: Burning - пока что ни где не используется
        private const string DamageUpName = "DAMAGE UP";         // _weaponHandler._projectilePrefab.Damage
        private const string FreezeName = "FREEZE";              // TODO: Freeze - нужно менять статы врагов
        private const string FullRecoveryName = "FULL RECOVERY"; // _health.FullRecovery(); - просто вызов метода
        private const string MagnetName = "MAGNET";              // TODO: Magnet - пока что не реализован
        private const string MaxHealthName = "MAX HEALTH";       // _health.Init(); - передаем int максимального здоровья
        private const string RegenerationName = "REGENERATION";  // TODO: Regeneration - пока что не реализован
        private const string ShootingName = "SHOOTING";          // _weaponHandler._weapon.ShootingDelay
        private const string SpeedUpName = "SPEED UP";           // TODO: SpeedUp - пока что не реализован
        private const string VampirismName = "VAMPIRISM";        // TODO: Vampirism - пока что не реализован
        
        private CollisionForBullets _collisionForBullets;
        private CollisionForEnemies _collisionForEnemies;
        private Stats _stats;

        public int ClipCapacityBullets => _weaponHandler.ClipCapacityBullets;
        public int CollectedBullets => _weaponHandler.CollectedBullets;

        public void Init(Stats stats)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
            
            _weaponHandler.Init(_stats.DamageStats);
            _health.Init(_stats.HealthStats.MaxHealth);
        }

        private void Awake()
        {
            _collisionForBullets = GetComponent<CollisionForBullets>();
            _collisionForBullets.Init(this);
            _collisionForEnemies = GetComponentInChildren<CollisionForEnemies>();
            _collisionForEnemies.Init(this);
        }

        private void Start() => 
            _bag.CreateClip(_weaponHandler.ClipCapacityBullets);

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
            _bag.CollectBullet(CollectedBullets);
        }

        private void OnShoted() => 
            _bag.UseCollectedBullets(CollectedBullets);
    }
}