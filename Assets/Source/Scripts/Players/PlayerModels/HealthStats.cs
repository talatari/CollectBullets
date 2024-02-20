using System;

namespace Source.Scripts.Players.PlayerModels
{
    [Serializable]
    public class HealthStats
    {
        private int _maxHealth;
        private float _regeneration;
        
        public HealthStats(int maxHealth, float regeneration)
        {
            if (maxHealth <= 0) 
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            if (regeneration < 0) 
                throw new ArgumentOutOfRangeException(nameof(regeneration));
            
            _maxHealth = maxHealth;
            _regeneration = regeneration;
        }
        
        public event Action<int> MaxHealthChanged;
        public event Action<float> RegenerationChanged;

        public int MaxHealth => _maxHealth;
        public float Regeneration => _regeneration;
        
        public void AddMaxHealth(float value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            // TODO: апкаст из-за цикла спавна патронов за спиной
            _maxHealth += (int)value;
            MaxHealthChanged?.Invoke(_maxHealth);
        }
        
        public void AddRegeneration(float value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _regeneration += value;
            RegenerationChanged?.Invoke(_regeneration);
        }
    }
}