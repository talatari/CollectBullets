using System;
using Source.Scripts.Players.PlayerStats;

namespace Source.Scripts.Players.Weapons
{
    public class Weapon
    {
        private readonly DamageStats _damageStats;
        private int _collectedBullets;
        private int _clipCapacityBullets;
        private float _shootingDelay;

        public Weapon(DamageStats damageStats)
        {
            _damageStats = damageStats ?? throw new ArgumentNullException(nameof(damageStats));
            
            _clipCapacityBullets = _damageStats.ClipCapacityBullets;
            _shootingDelay = _damageStats.ShootingDelay;
        }
        
        public int ClipCapacityBullets => _clipCapacityBullets;
        public int CollectedBullets => _collectedBullets;
        public float ShootingDelay => _shootingDelay;
        
        public void CollectBullet()
        {
            if (_collectedBullets < _clipCapacityBullets)
                _collectedBullets++;
        }

        public void Shoot()
        {
            if (_collectedBullets > 0)
                _collectedBullets--;
        }
    }
}