using System;
using System.Collections;
using Source.Scripts.Enemies;
using Source.Scripts.Infrastructure.Pools.Interfaces;
using Source.Scripts.Infrastructure.Spawners.Intefaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Infrastructure.Spawners
{
    public class SpawnerEnemy : MonoBehaviour, ISpawner
    {
        private IPool<Enemy> _poolEnemy;
        private Coroutine _coroutineSpawnEnemy;
        private float _spawnDelay;
        private int _maxEnemySpawnCount;
        private float _distanceRange;
        private int _spawnedCount;
        private int _spawCount;

        public void Init(IPool<Enemy> poolEnemy, int maxEnemySpawnCount, float distanceRange)
        {
            if (poolEnemy == null)
                throw new ArgumentNullException(nameof(poolEnemy));
            
            if (maxEnemySpawnCount < 0)
                throw new ArgumentOutOfRangeException(nameof(maxEnemySpawnCount));
            
            if (distanceRange <= 0) 
                throw new ArgumentOutOfRangeException(nameof(distanceRange));

            _poolEnemy = poolEnemy;
            _poolEnemy.Completed += OnWaveCompleted;
            _maxEnemySpawnCount = maxEnemySpawnCount;
            _distanceRange = distanceRange;
        }

        public event Action SpawnEnded;
        public event Action WaveCompleted;

        private void OnEnable() => 
            _poolEnemy?.Init();

        private void OnDisable() => 
            StopSpawn();

        private void OnDestroy() => 
            _poolEnemy.Completed -= OnWaveCompleted;

        public void StartSpawn(int waveNumber, int spawnCount, float spawnDelay)
        {
            StopSpawn();

            if (waveNumber <= 0) 
                throw new ArgumentOutOfRangeException(nameof(waveNumber));
            if (spawnCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(spawnCount));
            if (spawnDelay <= 0) 
                throw new ArgumentOutOfRangeException(nameof(spawnDelay));

            _spawCount = spawnCount;
            _spawnDelay = spawnDelay;
            _spawnedCount = 0;

            _coroutineSpawnEnemy = StartCoroutine(SpawnEnemy());
        }

        public void StopSpawn()
        {
            if (_coroutineSpawnEnemy != null)
                StopCoroutine(_coroutineSpawnEnemy);
        }

        public void Spawn()
        {
            if (_poolEnemy.AllItemsCount >= _maxEnemySpawnCount)
                return;
            
            Enemy enemy = _poolEnemy.Get();
            SetPosition(enemy);
            _spawnedCount++;
        }

        private void OnWaveCompleted() => 
            WaveCompleted?.Invoke();

        private void SetPosition(Enemy enemy)
        {
            float positionY = enemy.transform.position.y;
            enemy.transform.position = Random.insideUnitSphere * _distanceRange;

            enemy.transform.position = new Vector3(enemy.transform.position.x, positionY, enemy.transform.position.z);
        }

        private IEnumerator SpawnEnemy()
        {
            WaitForSeconds delay = new WaitForSeconds(_spawnDelay);

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