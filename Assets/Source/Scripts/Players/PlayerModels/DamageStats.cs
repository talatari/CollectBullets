using System;

namespace Source.Scripts.Players.PlayerModels
{
    [Serializable]
    public class DamageStats
    {
        private int _damage;
        private int _burning;
        private int _burningDuration;
        private int _vampirism;
        private int _clipCapacity;
        private int _shootingDelay;

        public DamageStats(int damage, int burning, int burningDuration, int vampirism, int clipCapacity, int shootingDelay)
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

            _damage = damage;
            _burning = burning;
            _burningDuration = burningDuration;
            _vampirism = vampirism;
            _clipCapacity = clipCapacity;
            _shootingDelay = shootingDelay;
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