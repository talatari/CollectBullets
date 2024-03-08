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
        private readonly ProjectileForPistol _projectileForPistolPrefab;

        public FactoryProjectile(ProjectileForPistol projectileForPistolPrefab, Transform parent)
        {
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
            _projectileForPistolPrefab = projectileForPistolPrefab
                ? projectileForPistolPrefab
                : throw new ArgumentNullException(nameof(projectileForPistolPrefab));
        }

        public ProjectileForPistol Create() => 
            Object.Instantiate(_projectileForPistolPrefab, _parent);
    }
}