using System;
using Source.Scripts.Enemies;
using Object = UnityEngine.Object;

namespace Source.Scripts.Infrastructure
{
    public class EnemyFactory
    {
        private readonly Enemy _enemyPrefab;

        public EnemyFactory(Enemy enemyPrefab) => 
            _enemyPrefab = enemyPrefab ? enemyPrefab : throw new ArgumentNullException(nameof(enemyPrefab));

        public Enemy Create() => 
            Object.Instantiate(_enemyPrefab);
    }
}