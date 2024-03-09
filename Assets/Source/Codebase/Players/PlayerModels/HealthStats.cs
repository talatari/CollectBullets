using System;

namespace Source.Codebase.Players.PlayerModels
{
    [Serializable]
    public class HealthStats
    {
        private int _defaultMaxHealth;
        private int _maxHealth;
        private int _defaultRegeneration;
        private int _regeneration;
        
        public HealthStats(int maxHealth, int regeneration)
        {
            if (maxHealth <= 0) 
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            if (regeneration < 0) 
                throw new ArgumentOutOfRangeException(nameof(regeneration));

            _defaultMaxHealth = maxHealth;
            _defaultRegeneration = regeneration;
            
            SetDefaultValues();
        }

        public event Action<int> MaxHealthChanged;
        public event Action<int> RegenerationChanged;

        public int MaxHealth => _maxHealth;
        public int Regeneration => _regeneration;
        
        public void SetDefaultValues()
        {
            _maxHealth = _defaultMaxHealth;
            MaxHealthChanged?.Invoke(_maxHealth);
            
            _regeneration = _defaultRegeneration;
            RegenerationChanged?.Invoke(_regeneration);
        }
        
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