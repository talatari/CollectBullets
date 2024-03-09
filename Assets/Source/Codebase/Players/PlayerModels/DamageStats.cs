using System;

namespace Source.Codebase.Players.PlayerModels
{
    [Serializable]
    public class DamageStats
    {
        private int _defaultDamage;
        private int _damage;
        private int _defaultBurning;
        private int _burning;
        private int _burningDuration;
        private int _defaultVampirism;
        private int _vampirism;
        private int _defaultClipCapacity;
        private int _clipCapacity;
        private int _defaultShootingDelay;
        private int _shootingDelay;

        public DamageStats(
            int damage, int burning, int burningDuration, int vampirism, int clipCapacity, int shootingDelay)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            if (burning < 0)
                throw new ArgumentOutOfRangeException(nameof(burning));
            if (burningDuration < 0)
                throw new ArgumentOutOfRangeException(nameof(burningDuration));
            if (vampirism < 0) 
                throw new ArgumentOutOfRangeException(nameof(vampirism));
            if (clipCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(clipCapacity));
            if (shootingDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(shootingDelay));
            
            _defaultDamage = damage;
            _defaultBurning = burning;
            _burningDuration = burningDuration;
            _defaultVampirism = vampirism;
            _defaultClipCapacity = clipCapacity;
            _defaultShootingDelay = shootingDelay;

            SetDefaultValues();
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
            _damage = _defaultDamage;
            DamageChanged?.Invoke(_damage);
            
            _burning = _defaultBurning;
            BurningChanged?.Invoke(_burning);
            
            _vampirism = _defaultVampirism;
            VampirismChanged?.Invoke(_vampirism);

            _clipCapacity = _defaultClipCapacity;
            ClipCapacityChanged?.Invoke(_clipCapacity);
            
            _shootingDelay = _defaultShootingDelay;
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