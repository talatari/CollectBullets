using System;
using System.Collections;
using Source.Codebase.Enemies;
using Source.Codebase.Infrastructure.Pools.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Codebase.Infrastructure.Spawners
{
    public class SpawnerEnemy : MonoBehaviour
    {
        private IPool<Enemy> _poolEnemy;
        private Coroutine _coroutineSpawnEnemy;
        private float _spawnDelay;
        private int _maxEnemySpawnCount;
        private float _distanceRange;
        private int _spawnedCount;
        private int _spawCount;
        private float _delayBetweenWaves;

        public void Init(IPool<Enemy> poolEnemy, int maxEnemySpawnCount, float distanceRange)
        {
            if (poolEnemy == null)
                throw new ArgumentNullException(nameof(poolEnemy));
            
            if (maxEnemySpawnCount < 0)
                throw new ArgumentOutOfRangeException(nameof(maxEnemySpawnCount));
            
            if (distanceRange <= 0) 
                throw new ArgumentOutOfRangeException(nameof(distanceRange));

            _poolEnemy = poolEnemy;
            _maxEnemySpawnCount = maxEnemySpawnCount;
            _distanceRange = distanceRange;
            
            _poolEnemy.Completed += OnCompleted;
        }

        public event Action SpawnEnded;
        public event Action Completed;
        
        private void OnDestroy()
        {
            _poolEnemy.Completed -= OnCompleted;
            
            StopSpawn();
        }

        public void StartSpawn(int waveNumber, int spawnCount, float spawnDelay, float delayBetweenWaves)
        {
            StopSpawn();

            if (waveNumber <= 0) 
                throw new ArgumentOutOfRangeException(nameof(waveNumber));
            if (spawnCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(spawnCount));
            if (spawnDelay <= 0) 
                throw new ArgumentOutOfRangeException(nameof(spawnDelay));
            if (delayBetweenWaves <= 0)
                throw new ArgumentOutOfRangeException(nameof(delayBetweenWaves));

            _spawCount = spawnCount;
            _spawnDelay = spawnDelay;
            _delayBetweenWaves = delayBetweenWaves;
            _spawnedCount = 0;

            print($"Started wave: {waveNumber} with {_spawCount} enemies");
            
            _coroutineSpawnEnemy = StartCoroutine(SpawnEnemy());
        }

        public void StopSpawn()
        {
            if (_coroutineSpawnEnemy != null)
                StopCoroutine(_coroutineSpawnEnemy);
        }

        public void ResetPool() => 
            _poolEnemy.ReleaseAll();

        private void OnCompleted()
        {
            if (_spawnedCount >= _spawCount)
                Completed?.Invoke();
        }

        private void Spawn()
        {
            if (_poolEnemy.AllItemsCount >= _maxEnemySpawnCount)
                return;
            
            Enemy enemy = _poolEnemy.Get();
            SetPosition(enemy);
            enemy.NavMeshAgentActive();
            _spawnedCount++;
        }

        private void SetPosition(Enemy enemy)
        {
            float positionY = enemy.transform.position.y;
            enemy.transform.position = Random.insideUnitSphere * _distanceRange;

            enemy.transform.position = new Vector3(enemy.transform.position.x, positionY, enemy.transform.position.z);
        }

        private IEnumerator SpawnEnemy()
        {
            WaitForSeconds delay = new WaitForSeconds(_spawnDelay);
            WaitForSeconds delayBetweenWaves = new WaitForSeconds(_delayBetweenWaves);
            
            yield return delayBetweenWaves;
            
            while (enabled)
            {
                if (_spawnedCount >= _spawCount)
                {
                    SpawnEnded?.Invoke();
                    
                    break;
                }
                
                Spawn();

                yield return delay;
            }
        }
    }
}