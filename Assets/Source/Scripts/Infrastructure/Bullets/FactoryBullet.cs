using System;
using Source.Scripts.Bullets;
using Object = UnityEngine.Object;

namespace Source.Scripts.Infrastructure.Bullets
{
    public class FactoryBullet
    {
        private readonly Bullet _bulletPrefab;

        public FactoryBullet(Bullet bulletPrefab) => 
            _bulletPrefab = bulletPrefab ? bulletPrefab : throw new ArgumentNullException(nameof(bulletPrefab));

        public Bullet Create() => 
            Object.Instantiate(_bulletPrefab);
    }
}