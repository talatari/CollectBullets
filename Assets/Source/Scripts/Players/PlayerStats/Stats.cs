using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class Stats
    {
        private DamageStats _damageStats;
        private HealthStats _healthStats;
        
        public DamageStats DamageStats => _damageStats;
        public HealthStats HealthStats => _healthStats;

        public void SetDamage(int damage)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            _damageStats.SetDamage(damage);
        }

        public void SetMaxHealth(int maxHealth)
        {
            if (maxHealth <= 0) 
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            
            _healthStats.SetMaxHealth(maxHealth);
        }
    }
}