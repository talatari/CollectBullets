using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Source.Enemies;
using Source.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Spawners
{
    public class EnemiesSpawner : ObjectPool<Enemy>
    {
        [SerializeField] private List<Enemy> _enemyPrefab;
        [SerializeField] private float _minRange = -50f;
        [SerializeField] private float _maxRange = 50f;
        [SerializeField] private int _minPoolSize = 5;
        [SerializeField] private int _maxPoolSize = 2000;
        [SerializeField] private float _spawnDelay = 1f;
        
        private Coroutine _spawnEnemy;
        
        private void Awake() => 
            CreatePool();

        private void OnEnable() => 
            _spawnEnemy = StartCoroutine(SpawnEnemy());

        private void OnDisable()
        {
            if (_spawnEnemy != null)
                StopCoroutine(_spawnEnemy);
        }

        private void CreatePool()
        {
            for (int i = 0; i < _minPoolSize; i++)
                GetObject(_enemyPrefab);
        }

        private void SetPosition(Enemy enemy)
        {
            float spawnPositionX = Random.Range(_maxRange, _minRange);
            float spawnPositionZ = Random.Range(_maxRange, _minRange);
            
            enemy.transform.position =  new Vector3(spawnPositionX, enemy.transform.position.y, spawnPositionZ);
        }

        private IEnumerator SpawnEnemy()
        {
            var delay = new WaitForSeconds(_spawnDelay);

            while (enabled)
            {
                foreach (Enemy enemy in Pool)
                {
                    if (enemy.gameObject.activeSelf == false)
                    {
                        enemy.gameObject.SetActive(true);
                        SetPosition(enemy);
                    }
                }

                if (Pool.Count() < _maxPoolSize)
                    GetObject(_enemyPrefab);
                
                yield return delay;

            }
        }
    }
}