using System;
using System.Collections.Generic;
using Source.Scripts.Enemies;

namespace Source.Scripts.Infrastructure.Enemies
{
    public class PoolEnemy
    {
        private readonly FactoryEnemy _factoryEnemy;
        private readonly int _startEnemyCount;

        private Queue<Enemy> _pool = new();
        private List<Enemy> _activeEnemies = new();

        public PoolEnemy(FactoryEnemy factoryEnemy, int startEnemyCount)
        {
            _factoryEnemy = factoryEnemy ?? throw new ArgumentNullException(nameof(factoryEnemy));
            
            if (startEnemyCount < 0) 
                throw new ArgumentOutOfRangeException(nameof(startEnemyCount));
            
            _startEnemyCount = startEnemyCount;
        }

        public int StartEnemyCount => _startEnemyCount;
        public int GetAmountAllEnemies => _activeEnemies.Count + _pool.Count;
        public List<Enemy> ActiveEnemies => _activeEnemies;

        public void Init()
        {
            for (int i = 0; i < _startEnemyCount; i++)
                Release(_factoryEnemy.Create());
        }

        public Enemy Get()
        {
            Enemy enemy;
            
            if (_pool.Count > 0)
            {
                enemy = _pool.Dequeue();
                SetActive(enemy);
                
                return enemy;
            }
            
            enemy = _factoryEnemy.Create();
            SetActive(enemy);

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

        public void SetActive(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
            _activeEnemies.Add(enemy);
        }
    }
}