using System;
using Source.Scripts.Bullets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Scripts.Infrastructure.Bullets
{
    public class FactoryBullet
    {
        private readonly Transform _bulletsParent;
        private readonly Bullet _bulletPrefab;

        public FactoryBullet(Bullet bulletPrefab, Transform bulletsParent)
        {
            _bulletsParent = bulletsParent ? bulletsParent : throw new ArgumentNullException(nameof(bulletsParent));
            _bulletPrefab = bulletPrefab ? bulletPrefab : throw new ArgumentNullException(nameof(bulletPrefab));
        }

        public Bullet Create() => 
            Object.Instantiate(_bulletPrefab, _bulletsParent);
    }
}