using System;

namespace Source.Codebase.Players.Weapons
{
    public class Weapon
    {
        private int _collectedBullets;
        private int _clipCapacity;

        public Weapon(int clipCapacity)
        {
            if (clipCapacity <= 0) 
                throw new ArgumentOutOfRangeException(nameof(clipCapacity));

            _clipCapacity = clipCapacity;
        }

        public event Action<int> CollectedBulletsChanged;

        public int ClipCapacity => _clipCapacity;
        public int CollectedBullets => _collectedBullets;

        public void CollectBullet()
        {
            if (_collectedBullets < _clipCapacity)
            {
                _collectedBullets++;
                CollectedBulletsChanged?.Invoke(_collectedBullets);
            }
        }

        public void Shoot()
        {
            if (_collectedBullets > 0)
            {
                _collectedBullets--;
                CollectedBulletsChanged?.Invoke(_collectedBullets);
            }
        }

        public void SetClipCapacity(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _clipCapacity = value;
        }
    }
}