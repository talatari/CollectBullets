using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class DamageStats
    {
        private int _damage;
        private int _burning;
        private int _clipCapacity;
        private float _shootingDelay;

        public DamageStats(int damage, int burning, int clipCapacity, float shootingDelay)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            if (burning < 0)
                throw new ArgumentOutOfRangeException(nameof(burning));
            if (clipCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(clipCapacity));
            if (shootingDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(shootingDelay));

            _damage = damage;
            _burning = burning;
            _clipCapacity = clipCapacity;
            _shootingDelay = shootingDelay;
        }

        public event Action<int> DamageChanged;
        public event Action<int> BurningChanged;
        public event Action<int> ClipCapacityChanged;
        public event Action<float> ShootingDelayChanged;

        public int Damage => _damage;
        public int Burning => _burning;
        public int ClipCapacity => _clipCapacity;
        public float ShootingDelay => _shootingDelay;

        public void ChangeDamage(int value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _damage = value;
            DamageChanged?.Invoke(_damage);
        }

        public void ChangeBurning(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _burning = value;
            BurningChanged?.Invoke(_burning);
        }

        public void ChangeClipCapacity(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _clipCapacity = value;
            ClipCapacityChanged?.Invoke(_clipCapacity);
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