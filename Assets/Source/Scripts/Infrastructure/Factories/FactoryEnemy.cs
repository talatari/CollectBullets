using System;
using Source.Scripts.Enemies;
using UnityEngine;

namespace Source.Scripts.Infrastructure.Factories
{
    public class FactoryEnemy : AbstractFactory
    {
        private readonly Enemy _enemyPrefab;
        private readonly TargetService _targetService;

        public FactoryEnemy(Enemy enemyPrefab, Transform parent, float distanceRange, TargetService targetService)
        {
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
            _enemyPrefab = enemyPrefab ? enemyPrefab : throw new ArgumentNullException(nameof(enemyPrefab));
            _targetService = targetService ?? throw new ArgumentNullException(nameof(targetService));

            if (distanceRange <= 0)
                throw new ArgumentOutOfRangeException(nameof(distanceRange));

            _distanceRange = distanceRange;
        }

        public Enemy Create()
        {
            Enemy enemy = CreateObject(_enemyPrefab);
            SetPosition(enemy);
            enemy.SetTarget(_targetService.Target.Position);

            return enemy;
        }

        private void SetPosition(Enemy enemy)
        {
            enemy.transform.position = GetPositionX(enemy.transform.position);
            enemy.transform.position = GetPositionZ(enemy.transform.position);
        }
    }
}