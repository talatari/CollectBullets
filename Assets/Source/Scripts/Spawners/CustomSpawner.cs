using System;
using System.Collections;
using Source.Scripts.Enemies;
using Source.Scripts.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Spawners
{
    public class CustomSpawner : MonoBehaviour
    {
        [SerializeField] private float _minRange = -50f;
        [SerializeField] private float _maxRange = 50f;
        [SerializeField] private int _maxSpawnCount = 100;
        
        private EnemyPool _enemyPool;
        private float _spawnDelay;
        
        public void Construct(EnemyPool enemyPool, float spawnDelay)
        {
            if (enemyPool == null) 
                throw new ArgumentNullException(nameof(enemyPool));
            
            if (spawnDelay < 0) 
                throw new ArgumentOutOfRangeException(nameof(spawnDelay));

            _enemyPool = enemyPool;
            _spawnDelay = spawnDelay;
            
            //TODO: remove before tests
            StartCoroutine(SpawnEnemy());
        }
        
        private IEnumerator SpawnEnemy()
        {
            var delay = new WaitForSeconds(_spawnDelay);

            while (enabled)
            {
                if (_enemyPool.ActiveCount >= _maxSpawnCount)
                    break;
                
                Enemy enemy = _enemyPool.Get();
                SetPosition(enemy);
                
                yield return delay;
            }
        }

        private void SetPosition(Enemy enemy)
        {
            float spawnPositionX = Random.Range(_maxRange, _minRange);
            float spawnPositionZ = Random.Range(_maxRange, _minRange);
            
            enemy.transform.position =  new Vector3(spawnPositionX, enemy.transform.position.y, spawnPositionZ);
        }
    }
}