using System;
using Source.Scripts.Enemies;
using Object = UnityEngine.Object;

namespace Source.Scripts.Infrastructure.Enemies
{
    public class FactoryEnemy
    {
        private readonly Enemy _enemyPrefab;

        public FactoryEnemy(Enemy enemyPrefab) => 
            _enemyPrefab = enemyPrefab ? enemyPrefab : throw new ArgumentNullException(nameof(enemyPrefab));

        public Enemy Create() => 
            Object.Instantiate(_enemyPrefab);
    }
}