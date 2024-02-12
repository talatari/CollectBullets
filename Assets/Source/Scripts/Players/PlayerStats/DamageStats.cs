using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class DamageStats
    {
        private int _damage;
        private int _clipCapacityBullets;
        
        public DamageStats(int damage, int clipCapacityBullets)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            if (clipCapacityBullets <= 0)
                throw new ArgumentOutOfRangeException(nameof(clipCapacityBullets));
            
            _damage = damage;
            _clipCapacityBullets = clipCapacityBullets;
        }
        
        public int Damage => _damage;
        public int ClipCapacityBullets => _clipCapacityBullets;
    }
}