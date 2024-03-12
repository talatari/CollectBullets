using System;
using Source.Codebase.Infrastructure.Factories.Interfaces;
using Source.Codebase.Players.Projectiles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Codebase.Infrastructure.Factories
{
    // TODO: implement
    public class FactoryProjectile : IFactory<ProjectileForPistol>
    {
        private readonly Transform _parent;
        private readonly ProjectileForPistol _prefab;

        public FactoryProjectile(ProjectileForPistol prefab, Transform parent)
        {
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
            _prefab = prefab ? prefab : throw new ArgumentNullException(nameof(prefab));
        }

        public ProjectileForPistol Create() => 
            Object.Instantiate(_prefab, _parent);
    }
}