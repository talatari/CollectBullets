using System;
using System.Collections.Generic;
using Source.Scripts.Enemies;
using Source.Scripts.Infrastructure;

namespace Source.Scripts.Pool
{
    public class EnemyPool
    {
        private readonly EnemyFactory _enemyFactory;

        private Queue<Enemy> _pool = new();
        private List<Enemy> _activeEnemies = new();

        public EnemyPool(EnemyFactory enemyFactory) => 
            _enemyFactory = enemyFactory ?? throw new ArgumentNullException(nameof(enemyFactory));

        public int ActiveCount => _activeEnemies.Count;
        
        public Enemy Get()
        {
            Enemy enemy;
            
            if (_pool.Count > 0)
            {
                enemy = _pool.Dequeue();
                SetEnemyActive(enemy);
                
                return enemy;
            }
            
            enemy = _enemyFactory.Create();
            SetEnemyActive(enemy);

            return enemy;
        }

        public void Release(Enemy enemy)
        {
            if (enemy == null) 
                throw new ArgumentNullException(nameof(enemy));
            
            enemy.gameObject.SetActive(false);
            _pool.Enqueue(enemy);
            _activeEnemies.Remove(enemy);
        }

        private void SetEnemyActive(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
            _activeEnemies.Add(enemy);
        }
    }
}