using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class DamageStats
    {
        private int _damage;
        private int _clipCapacity;
        private int _burning;
        private float _shootingDelay;

        public DamageStats(int damage, int clipCapacity, int burning, float shootingDelay)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            if (clipCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(clipCapacity));
            if (burning < 0)
                throw new ArgumentOutOfRangeException(nameof(burning));
            if (shootingDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(shootingDelay));

            _damage = damage;
            _clipCapacity = clipCapacity;
            _burning = burning;
            _shootingDelay = shootingDelay;
        }

        public event Action<int> DamageChanged;
        public event Action<int> ClipCapacityChanged;
        public event Action<int> BurningChanged;
        public event Action<float> ShootingDelayChanged;

        public int Damage => _damage;
        public int ClipCapacity => _clipCapacity;
        public int Burning => _burning;
        public float ShootingDelay => _shootingDelay;

        public void ChangeDamage(int value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _damage = value;
            DamageChanged?.Invoke(_damage);
        }

        public void ChangeClipCapacity(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _clipCapacity = value;
            ClipCapacityChanged?.Invoke(_clipCapacity);
        }
        
        public void ChangeBurning(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _burning = value;
            BurningChanged?.Invoke(_burning);
        }
        
        public void ChangeShootingDelay(float value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _shootingDelay = value;
            ShootingDelayChanged?.Invoke(_shootingDelay);
        }
    }
}