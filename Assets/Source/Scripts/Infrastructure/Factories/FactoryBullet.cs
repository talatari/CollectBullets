using System;
using Source.Scripts.Bullets;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Source.Scripts.Infrastructure.Factories
{
    public class FactoryBullet
    {
        private readonly Transform _bulletsParent;
        private readonly Bullet _bulletPrefab;

        private float _distanceRange;

        public FactoryBullet(Bullet bulletPrefab, Transform bulletsParent, float distanceRange)
        {
            _bulletsParent = bulletsParent ? bulletsParent : throw new ArgumentNullException(nameof(bulletsParent));
            _bulletPrefab = bulletPrefab ? bulletPrefab : throw new ArgumentNullException(nameof(bulletPrefab));
            
            if (distanceRange <= 0)
                throw new ArgumentOutOfRangeException(nameof(distanceRange));
            
            _distanceRange = distanceRange;
        }

        public Bullet Create()
        {
            Bullet bullet = Object.Instantiate(_bulletPrefab, _bulletsParent);
            SetPosition(bullet);

            return bullet;
        }
        
        private void SetPosition(Bullet bullet)
        {
            float spawnPositionX = Random.Range(-1 * _distanceRange, _distanceRange);
            float spawnPositionY = Random.Range(0, _distanceRange);
            float spawnPositionZ = Random.Range(-1 * _distanceRange, _distanceRange);

            bullet.transform.position = new Vector3(spawnPositionX, spawnPositionY, spawnPositionZ);
        }
    }
}