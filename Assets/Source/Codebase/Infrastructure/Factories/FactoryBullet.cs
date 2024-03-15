using System;
using Source.Codebase.Bullets;
using Source.Codebase.Infrastructure.Factories.Interfaces;
using Source.Codebase.Infrastructure.Services;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Codebase.Infrastructure.Factories
{
    public class FactoryBullet : IFactory<Bullet>
    {
        private readonly Transform _parent;
        private readonly Bullet _bulletPrefab;
        private readonly TargetProvider _targetProvider;

        public FactoryBullet(Bullet bulletPrefab, Transform parent, TargetProvider targetProvider)
        {
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
            _bulletPrefab = bulletPrefab ? bulletPrefab : throw new ArgumentNullException(nameof(bulletPrefab));
            _targetProvider = targetProvider ?? throw new ArgumentNullException(nameof(targetProvider));
        }

        public Bullet Create()
        {
            
            Bullet bullet = Object.Instantiate(_bulletPrefab, _parent);
            bullet.SetTarget(_targetProvider.Target);
            
            return bullet;
        }
    }
}