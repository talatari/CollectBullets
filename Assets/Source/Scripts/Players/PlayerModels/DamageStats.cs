using System;

namespace Source.Scripts.Players.PlayerModels
{
    [Serializable]
    public class DamageStats
    {
        private int _damage;
        private float _burning;
        private float _vampirism;
        private int _clipCapacity;
        private float _shootingDelay;

        public DamageStats(int damage, float burning, float vampirism, int clipCapacity, float shootingDelay)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            if (burning < 0)
                throw new ArgumentOutOfRangeException(nameof(burning));
            if (vampirism < 0) 
                throw new ArgumentOutOfRangeException(nameof(vampirism));
            if (clipCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(clipCapacity));
            if (shootingDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(shootingDelay));

            _damage = damage;
            _burning = burning;
            _vampirism = vampirism;
            _clipCapacity = clipCapacity;
            _shootingDelay = shootingDelay;
        }

        public event Action<int> DamageChanged;
        public event Action<float> BurningChanged;
        public event Action<float> VampirismChanged;
        public event Action<int> ClipCapacityChanged;
        public event Action<float> ShootingDelayChanged;

        public int Damage => _damage;
        public float Burning => _burning;
        public float Vampirism => _vampirism;
        public int ClipCapacity => _clipCapacity;
        public float ShootingDelay => _shootingDelay;

        public void AddDamage(float value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            // TODO: апкаст из-за целочисленного цикла спавна обоймы за спиной
            _damage += (int)value;
            DamageChanged?.Invoke(_damage);
        }

        public void AddBurning(float value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _burning += value;
            BurningChanged?.Invoke(_burning);
        }

        public void AddVampirism(float value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _vampirism += value;
            VampirismChanged?.Invoke(_vampirism);
        }
        
        public void AddClipCapacity(float value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            // TODO: пришлось апкастить, т.к. на целочисленное значение обоймы завязан цикл спавна патронов за спиной
            _clipCapacity += (int)value;
            ClipCapacityChanged?.Invoke(_clipCapacity);
        }

        public void AddShootingDelay(float value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _shootingDelay -= value;
            ShootingDelayChanged?.Invoke(_shootingDelay);
        }
    }
}