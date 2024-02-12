using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class HealthStats
    {
        private int _maxHealth;
        private float _regeneration;
        private float _vampirism;
        
        public HealthStats(int maxHealth, float regeneration, float vampirism)
        {
            if (maxHealth <= 0) 
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            if (regeneration <= 0) 
                throw new ArgumentOutOfRangeException(nameof(regeneration));
            if (vampirism <= 0) 
                throw new ArgumentOutOfRangeException(nameof(vampirism));

            _maxHealth = maxHealth;
            _regeneration = regeneration;
            _vampirism = vampirism;
        }
        
        public event Action<int> MaxHealthChanged;
        public event Action<float> RegenerationChanged;
        public event Action<float> VampirismChanged;

        public int MaxHealth => _maxHealth;
        public float Regeneration => _regeneration;
        public float Vampirism => _vampirism;
        
        public void ChangeMaxHealth(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _maxHealth = value;
            MaxHealthChanged?.Invoke(_maxHealth);
        }
        
        public void ChangeRegeneration(float value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _regeneration = value;
            RegenerationChanged?.Invoke(_regeneration);
        }
        
        public void ChangeVampirism(float value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _vampirism = value;
            VampirismChanged?.Invoke(_vampirism);
        }
    }
}