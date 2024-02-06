using System;
using Source.Scripts.Enemies;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Source.Scripts.Infrastructure.Factories
{
    public class FactoryEnemy
    {
        private readonly Transform _enemiesParent;
        private readonly Enemy _enemyPrefab;
        private float _distanceRange;
        private readonly TargetService _targetService;

        public FactoryEnemy(Enemy enemyPrefab, Transform enemiesParent, float distanceRange,
            TargetService targetService)
        {
            _enemiesParent = enemiesParent ? enemiesParent : throw new ArgumentNullException(nameof(enemiesParent));
            _enemyPrefab = enemyPrefab ? enemyPrefab : throw new ArgumentNullException(nameof(enemyPrefab));
            _targetService = targetService ?? throw new ArgumentNullException(nameof(targetService));

            if (distanceRange <= 0)
                throw new ArgumentOutOfRangeException(nameof(distanceRange));

            _distanceRange = distanceRange;
        }

        public Enemy Create()
        {
            Enemy enemy = Object.Instantiate(_enemyPrefab, _enemiesParent);
            SetPosition(enemy);
            enemy.SetTarget(_targetService.Target.Position);

            return enemy;
        }

        private void SetPosition(Enemy enemy)
        {
            float spawnPositionX = Random.Range(-1 * _distanceRange, _distanceRange);
            float spawnPositionZ = Random.Range(-1 * _distanceRange, _distanceRange);

            enemy.transform.position = new Vector3(spawnPositionX, enemy.transform.position.y, spawnPositionZ);
        }
    }
}