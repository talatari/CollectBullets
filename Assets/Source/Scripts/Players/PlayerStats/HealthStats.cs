using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class HealthStats
    {
        private int _maxHealth;
        
        public int MaxHealth => _maxHealth;

        public HealthStats(int maxHealth)
        {
            if (maxHealth <= 0) 
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            
            _maxHealth = maxHealth;
        }
    }
}