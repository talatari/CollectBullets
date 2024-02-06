using System;
using System.Collections;
using System.Collections.Generic;
using Source.Scripts.Enemies;
using Source.Scripts.Infrastructure.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Spawners
{
    public class SpawnerEnemy : MonoBehaviour
    {
        private PoolEnemy _poolEnemy;
        private Coroutine _coroutineSpawnEnemy;
        private float _spawnDelay;
        private int _maxEnemySpawnCount;

        public void Construct(PoolEnemy poolEnemy, float spawnDelay, int maxEnemySpawnCount)
        {
            if (poolEnemy == null) 
                throw new ArgumentNullException(nameof(poolEnemy));
            
            if (spawnDelay < 0) 
                throw new ArgumentOutOfRangeException(nameof(spawnDelay));
            
            if (maxEnemySpawnCount < 0) 
                throw new ArgumentOutOfRangeException(nameof(maxEnemySpawnCount));

            _poolEnemy = poolEnemy;
            _spawnDelay = spawnDelay;
            _maxEnemySpawnCount = maxEnemySpawnCount;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                Spawn();
            
            if (Input.GetKeyDown(KeyCode.R))
                Release();
        }

        private void OnDisable() => 
            StopSpawn();

        private void Release()
        {
            List<Enemy> activeEnemies = _poolEnemy.ActiveEnemies;
            int randomIndex = Random.Range(0, activeEnemies.Count);
            
            if (randomIndex < 0 || randomIndex >= activeEnemies.Count)
                return;
            
            _poolEnemy.Release(activeEnemies[randomIndex]);
        }

        public void StartSpawn()
        {
            StopSpawn();

            _coroutineSpawnEnemy = StartCoroutine(SpawnEnemy());
        }

        public void StopSpawn()
        {
            if (_coroutineSpawnEnemy != null)
                StopCoroutine(_coroutineSpawnEnemy);
        }

        private IEnumerator SpawnEnemy()
        {
            WaitForSeconds delay = new WaitForSeconds(_spawnDelay);

            while (enabled)
            {
                if (_poolEnemy.ActiveEnemies.Count >= _poolEnemy.StartEnemyCount)
                    break;
                
                Spawn();

                yield return delay;
            }
        }

        private void Spawn()
        {
            if (_poolEnemy.GetAmountAllEnemies >= _maxEnemySpawnCount)
                return;
            
            _poolEnemy.Get();
        }
    }
}