using System;
using Source.Scripts.Bullets;
using UnityEngine;

namespace Source.Scripts.Infrastructure.Factories
{
    public class FactoryBullet : AbstractFactory
    {
        private readonly Bullet _bulletPrefab;

        public FactoryBullet(Bullet bulletPrefab, Transform parent, float distanceRange)
        {
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
            _bulletPrefab = bulletPrefab ? bulletPrefab : throw new ArgumentNullException(nameof(bulletPrefab));
            
            if (distanceRange <= 0)
                throw new ArgumentOutOfRangeException(nameof(distanceRange));
            
            _distanceRange = distanceRange;
        }

        public Bullet Create()
        {
            Bullet bullet = CreateObject(_bulletPrefab);
            SetPosition(bullet);
            
            return bullet;
        }
        
        private void SetPosition(Bullet bullet)
        {
            bullet.transform.position = GetPositionX(bullet.transform.position);
            bullet.transform.position = GetPositionY(bullet.transform.position);
            bullet.transform.position = GetPositionZ(bullet.transform.position);
        }
    }
}