using System;
using Source.Scripts.Players.CollisionHandlers;
using Source.Scripts.Players.Joystick;
using Source.Scripts.Players.Movement;
using Source.Scripts.Players.Weapons;
using UnityEngine;

namespace Source.Scripts.Players
{
    [RequireComponent(typeof(CollisionForBullets), typeof(Mover))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Bag _bag;
        [SerializeField] private JoystickForRotator _joystickForRotator;
        [SerializeField] private WeaponHandler _weaponHandler;
        [SerializeField] private Health _health;
        
        private CollisionForBullets _collisionForBullets;
        private CollisionForEnemies _collisionForEnemies;
        
        public int ClipCapacityBullets => _weaponHandler.ClipCapacityBullets;
        public int CollectedBullets => _weaponHandler.CollectedBullets;
        
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