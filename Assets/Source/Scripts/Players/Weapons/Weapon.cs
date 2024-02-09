using System;
using Source.Scripts.Players.Weapons.SO;

namespace Source.Scripts.Players.Weapons
{
    public class Weapon
    {
        private int _clipCapacityBullets;
        private int _collectedBullets;
        private float _shootingDelay;

        public Weapon(WeaponScriptableObject weaponScriptableObject)
        {
           if (weaponScriptableObject.ClipCapacityProjectile <= 0)
               throw new ArgumentOutOfRangeException(nameof(weaponScriptableObject.ClipCapacityProjectile));
           if (weaponScriptableObject.ShootingDelay < 0)
               throw new ArgumentOutOfRangeException(nameof(weaponScriptableObject.ShootingDelay));
           
           _clipCapacityBullets = weaponScriptableObject.ClipCapacityProjectile;
           _shootingDelay = weaponScriptableObject.ShootingDelay;
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