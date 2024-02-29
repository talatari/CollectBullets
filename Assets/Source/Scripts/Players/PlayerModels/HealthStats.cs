using System;

namespace Source.Scripts.Players.PlayerModels
{
    [Serializable]
    public class HealthStats
    {
        private int _maxHealth;
        private int _regeneration;
        
        public HealthStats(int maxHealth, int regeneration)
        {
            if (maxHealth <= 0) 
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            if (regeneration < 0) 
                throw new ArgumentOutOfRangeException(nameof(regeneration));
            
            _maxHealth = maxHealth;
            _regeneration = regeneration;
        }
        
        public event Action<int> MaxHealthChanged;
        public event Action<int> RegenerationChanged;

        public int MaxHealth => _maxHealth;
        public int Regeneration => _regeneration;
        
        public int AddMaxHealth(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _maxHealth += value;
            MaxHealthChanged?.Invoke(_maxHealth);

            return _maxHealth;
        }
        
        public int AddRegeneration(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _regeneration += value;
            RegenerationChanged?.Invoke(_regeneration);

            return _regeneration;
        }
    }
}