using System;
using Source.Scripts.Enemies;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Scripts.Infrastructure.Enemies
{
    public class FactoryEnemy
    {
        private readonly Transform _enemiesParent;
        private readonly Enemy _enemyPrefab;

        public FactoryEnemy(Enemy enemyPrefab, Transform enemiesParent)
        {
            _enemiesParent = enemiesParent ? enemiesParent : throw new ArgumentNullException(nameof(enemiesParent));
            _enemyPrefab = enemyPrefab ? enemyPrefab : throw new ArgumentNullException(nameof(enemyPrefab));
        }

        public Enemy Create() => 
            Object.Instantiate(_enemyPrefab, _enemiesParent);
    }
}