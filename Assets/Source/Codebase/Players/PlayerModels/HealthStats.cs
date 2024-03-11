using System;
using Source.Codebase.SO;

namespace Source.Codebase.Players.PlayerModels
{
    [Serializable]
    public class HealthStats
    {
        private readonly PlayerScriptableObject _playerConfig;
        private int _maxHealth;
        private int _regeneration;

        public HealthStats(PlayerScriptableObject playerConfig)
        {
            _playerConfig = playerConfig ? playerConfig : throw new ArgumentNullException(nameof(playerConfig));
            if (_playerConfig.MaxHealth <= 0) 
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.MaxHealth));
            if (_playerConfig.Regeneration < 0) 
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.Regeneration));

            _maxHealth = _playerConfig.MaxHealth;
            _regeneration = _playerConfig.Regeneration;
        }

        public event Action<int> MaxHealthChanged;
        public event Action<int> RegenerationChanged;

        public int MaxHealth => _maxHealth;
        public int Regeneration => _regeneration;
        
        public void SetDefaultValues()
        {
            _maxHealth = _playerConfig.MaxHealth;
            MaxHealthChanged?.Invoke(_maxHealth);
            
            _regeneration = _playerConfig.Regeneration;
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