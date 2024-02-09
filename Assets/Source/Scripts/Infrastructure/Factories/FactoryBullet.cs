using System;
using Source.Scripts.Infrastructure.Factories.Interfaces;
using Source.Scripts.Players;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Scripts.Infrastructure.Factories
{
    public class FactoryBullet : IFactory<Bullet>
    {
        private readonly Transform _parent;
        private readonly Bullet _bulletPrefab;

        public FactoryBullet(Bullet bulletPrefab, Transform parent)
        {
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
            _bulletPrefab = bulletPrefab ? bulletPrefab : throw new ArgumentNullException(nameof(bulletPrefab));
        }

        public Bullet Create() => 
            Object.Instantiate(_bulletPrefab, _parent);
    }
}