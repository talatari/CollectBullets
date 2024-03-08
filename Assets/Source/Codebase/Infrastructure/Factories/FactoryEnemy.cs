using System;
using System.Collections.Generic;
using Source.Codebase.Enemies;
using Source.Codebase.Infrastructure.Factories.Interfaces;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Players.PlayerModels;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Source.Codebase.Infrastructure.Factories
{
    public class FactoryEnemy : IFactory<Enemy>
    {
        private readonly List<Enemy> _enemyPrefabs;
        private readonly TargetProvider _targetProvider;
        private readonly Transform _parent;
        private readonly CommonStats _commonStats;

        public FactoryEnemy(
            List<Enemy> enemyPrefabses, Transform parent, TargetProvider targetProvider, CommonStats commonStats)
        {
            _enemyPrefabs = enemyPrefabses ?? throw new ArgumentNullException(nameof(enemyPrefabses));
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
            _targetProvider = targetProvider ?? throw new ArgumentNullException(nameof(targetProvider));
            _commonStats = commonStats ?? throw new ArgumentNullException(nameof(commonStats));
        }

        public Enemy Create()
        {
            int randomIndex = Random.Range(0, _enemyPrefabs.Count);
            Enemy enemy = Object.Instantiate(_enemyPrefabs[randomIndex], _parent);
            enemy.SetTarget(_targetProvider.Target, _commonStats);
            enemy.name = _enemyPrefabs[randomIndex].name + enemy.GetInstanceID();

            return enemy;
        }
    }
}