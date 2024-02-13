using System;

namespace Source.Scripts.Players.Weapons
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

        public void SetClipCapacity(int clipCapacity) => 
            _clipCapacity = clipCapacity;
    }
}