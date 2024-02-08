using System;

namespace Source.Scripts.Players.Weapons
{
    public class Weapon
    {
        private int _clipCapacityBullets;
        private int _collectedBullets;
        private float _shootingDelay;

        public Weapon(Pistol pistol)
        {
           if (pistol.ClipCapacityBullets <= 0)
               throw new ArgumentOutOfRangeException(nameof(pistol.ClipCapacityBullets));
           if (pistol.CollectedBullets < 0)
               throw new ArgumentOutOfRangeException(nameof(pistol.CollectedBullets));
           if (pistol.ShootingDelay < 0)
               throw new ArgumentOutOfRangeException(nameof(pistol.ShootingDelay));
           
           _clipCapacityBullets = pistol.ClipCapacityBullets;
           _collectedBullets = pistol.CollectedBullets;
           _shootingDelay = pistol.ShootingDelay;
        }
        
        public int ClipCapacityBullets => _clipCapacityBullets;
        public int CollectedBullets => _collectedBullets;
        public float ShootingDelay => _shootingDelay;
        
        public void CollectBullet()
        {
            if (_collectedBullets < _clipCapacityBullets)
                _collectedBullets++;
        }

        public void Shoot()
        {
            if (_collectedBullets > 0)
                _collectedBullets--;
        }
    }
}