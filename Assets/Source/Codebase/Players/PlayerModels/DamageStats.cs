using System;
using Source.Codebase.SO;

namespace Source.Codebase.Players.PlayerModels
{
    [Serializable]
    public class DamageStats
    {
        private readonly PlayerScriptableObject _playerConfig;
        private int _damage;
        private int _burning;
        private int _burningDuration;
        private int _vampirism;
        private int _clipCapacity;
        private int _shootingDelay;

        public DamageStats(PlayerScriptableObject playerConfig)
        {
            _playerConfig = playerConfig ? playerConfig : throw new ArgumentNullException(nameof(playerConfig));
            if (_playerConfig.Damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.Damage));
            if (_playerConfig.Burning < 0)
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.Burning));
            if (_playerConfig.BurningDuration <= 0)
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.BurningDuration));
            if (_playerConfig.Vampirism < 0) 
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.Vampirism));
            if (_playerConfig.ClipCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.ClipCapacity));
            if (_playerConfig.ShootingDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.ShootingDelay));
            
            _damage = _playerConfig.Damage;
            _burning = _playerConfig.Burning;
            _burningDuration = _playerConfig.BurningDuration;
            _vampirism = _playerConfig.Vampirism;
            _clipCapacity = _playerConfig.ClipCapacity;
            _shootingDelay = _playerConfig.ShootingDelay;
        }

        public event Action<int> DamageChanged;
        public event Action<int> BurningChanged;
        public event Action<int> VampirismChanged;
        public event Action<int> ClipCapacityChanged;
        public event Action<int> ShootingDelayChanged;

        public int Damage => _damage;
        public int Burning => _burning;
        public int BurningDuration => _burningDuration;
        public int Vampirism => _vampirism;
        public int ClipCapacity => _clipCapacity;
        public int ShootingDelay => _shootingDelay;

        public void SetDefaultValues()
        {
            _damage = _playerConfig.Damage;
            DamageChanged?.Invoke(_damage);
            
            _burning = _playerConfig.Burning;
            BurningChanged?.Invoke(_burning);
            
            _vampirism = _playerConfig.Vampirism;
            VampirismChanged?.Invoke(_vampirism);

            _clipCapacity = _playerConfig.ClipCapacity;
            ClipCapacityChanged?.Invoke(_clipCapacity);
            
            _shootingDelay = _playerConfig.ShootingDelay;
            ShootingDelayChanged?.Invoke(_shootingDelay);
        }
        
        public int AddDamage(int value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _damage += value;
            DamageChanged?.Invoke(_damage);

            return _damage;
        }

        public int AddBurning(int value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _burning += value;
            BurningChanged?.Invoke(_burning);
            
            return _burning;
        }

        public int AddVampirism(int value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _vampirism += value;
            VampirismChanged?.Invoke(_vampirism);

            return _vampirism;
        }
        
        public int AddClipCapacity(int value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _clipCapacity += value;
            ClipCapacityChanged?.Invoke(_clipCapacity);

            return _clipCapacity;
        }

        public int AddShootingDelay(int value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _shootingDelay += value;
            ShootingDelayChanged?.Invoke(_shootingDelay);

            return _shootingDelay;
        }
    }
}