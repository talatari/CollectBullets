using System;
using Source.Scripts.Players.PlayerStats;

namespace Source.Scripts.Players.Weapons
{
    public class Weapon
    {
        private DamageStats _damageStats;
        private int _collectedBullets;
        private int _clipCapacity;

        public Weapon(DamageStats damageStats)
        {
            _damageStats = damageStats ?? throw new ArgumentNullException(nameof(damageStats));
            
            _clipCapacity = _damageStats.ClipCapacity;
            // TODO: где-то нужно отписаться
            _damageStats.ClipCapacityChanged += OnClipCapacityChanged;
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

        private void OnClipCapacityChanged(int clipCapacity) => 
            _clipCapacity = clipCapacity;
    }
}