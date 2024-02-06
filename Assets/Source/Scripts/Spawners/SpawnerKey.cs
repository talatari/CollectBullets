using System;
using System.Collections;
using System.Collections.Generic;
using Source.Scripts.Enemies;
using Source.Scripts.Infrastructure.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Spawners
{
    public class SpawnerKey : MonoBehaviour
    {
        private Pool<Key> _poolEnemy;
        private Coroutine _coroutineSpawnEnemy;
        private float _spawnDelay;
        private float _distanceRange;
        private int _maxEnemySpawnCount;
    // todo: rename naming
        public void Construct(Pool<Key> poolEnemy, float spawnDelay, float distanceRange, int maxEnemySpawnCount)
        {
            if (poolEnemy == null) 
                throw new ArgumentNullException(nameof(poolEnemy));
            
            if (spawnDelay < 0) 
                throw new ArgumentOutOfRangeException(nameof(spawnDelay));
            
            if (distanceRange <= 0)
                throw new ArgumentOutOfRangeException(nameof(distanceRange));
            
            if (maxEnemySpawnCount < 0) 
                throw new ArgumentOutOfRangeException(nameof(maxEnemySpawnCount));

            _poolEnemy = poolEnemy;
            _spawnDelay = spawnDelay;
            _distanceRange = distanceRange;
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
            List<Key> activeEnemies = _poolEnemy.ActiveItems;
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
                if (_poolEnemy.ActiveItems.Count >= _poolEnemy.StartItemCount)
                    break;
                
                Spawn();

                yield return delay;
            }
        }

        private void Spawn()
        {
            if (_poolEnemy.AllItemsCount >= _maxEnemySpawnCount)
                return;
            
            // Enemy enemy = _poolEnemy.Get();
            // SetPosition(enemy);
        }
        //
        // private void SetPosition(Enemy enemy)
        // {
        //     float spawnPositionX = Random.Range(-1 * _distanceRange, _distanceRange);
        //     float spawnPositionZ = Random.Range(-1 * _distanceRange, _distanceRange);
        //     
        //     enemy.transform.position =  new Vector3(spawnPositionX, enemy.transform.position.y, spawnPositionZ);
        // }
    }
}