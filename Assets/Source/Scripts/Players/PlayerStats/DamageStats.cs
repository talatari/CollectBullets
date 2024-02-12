using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class DamageStats
    {
        private int _damage;
        private int _clipCapacityBullets;
        private int _burning;
        private float _shootingDelay;
        
        public DamageStats(int damage, int clipCapacityBullets, int burning, float shootingDelay)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            if (clipCapacityBullets <= 0)
                throw new ArgumentOutOfRangeException(nameof(clipCapacityBullets));
            if (burning <= 0)
                throw new ArgumentOutOfRangeException(nameof(burning));
            if (shootingDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(shootingDelay));

            _damage = damage;
            _clipCapacityBullets = clipCapacityBullets;
            _burning = burning;
            _shootingDelay = shootingDelay;
        }
        
        public int Damage => _damage;
        public int ClipCapacityBullets => _clipCapacityBullets;
        public int Burning => _burning;
        public float ShootingDelay => _shootingDelay;
    }
}