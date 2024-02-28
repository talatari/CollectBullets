using System;
using System.Collections.Generic;
using Source.Scripts.Enemies;
using Source.Scripts.Infrastructure.Factories.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Source.Scripts.Infrastructure.Factories
{
    public class FactoryEnemy : IFactory<Enemy>
    {
        private readonly List<Enemy> _enemyPrefabs;
        private readonly TargetProvider _targetProvider;
        private readonly Transform _parent;

        public FactoryEnemy(List<Enemy> enemyPrefabses, Transform parent, TargetProvider targetProvider)
        {
            _enemyPrefabs = enemyPrefabses ?? throw new ArgumentNullException(nameof(enemyPrefabses));
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
            _targetProvider = targetProvider ?? throw new ArgumentNullException(nameof(targetProvider));
        }

        public Enemy Create()
        {
            int randomIndex = Random.Range(0, _enemyPrefabs.Count);
            Enemy enemy = Object.Instantiate(_enemyPrefabs[randomIndex], _parent);
            enemy.SetTarget(_targetProvider.Target);
            enemy.name = _enemyPrefabs[randomIndex].name + enemy.GetInstanceID();

            return enemy;
        }
    }
}