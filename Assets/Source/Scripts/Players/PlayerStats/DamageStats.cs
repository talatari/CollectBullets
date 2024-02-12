using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class DamageStats
    {
        private int _damage;
        
        public int Damage => _damage;
        
        public void SetDamage(int damage)
        {
            if (damage <= 0)
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            _damage = damage;
        }
    }
}