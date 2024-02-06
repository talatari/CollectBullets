using System;
using Source.Scripts.Enemies;
using Source.Scripts.Infrastructure.Factories.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Scripts.Infrastructure.Factories
{
    public class FactoryEnemy : IFactory<Enemy>
    {
        private readonly Enemy _enemyPrefab;
        private readonly TargetProvider _targetProvider;
        private readonly Transform _parent;

        public FactoryEnemy(Enemy enemyPrefab, Transform parent, TargetProvider targetProvider)
        {
            _enemyPrefab = enemyPrefab ? enemyPrefab : throw new ArgumentNullException(nameof(enemyPrefab));
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
            _targetProvider = targetProvider ?? throw new ArgumentNullException(nameof(targetProvider));
        }

        public Enemy Create()
        {
            Enemy enemy = Object.Instantiate(_enemyPrefab, _parent);
            enemy.SetTarget(_targetProvider.Target);

            return enemy;
        }
    }
}