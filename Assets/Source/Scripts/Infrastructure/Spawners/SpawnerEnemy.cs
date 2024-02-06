using System;
using System.Collections;
using System.Collections.Generic;
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

        public void Construct(IPool<Enemy> poolEnemy, float spawnDelay, int maxEnemySpawnCount, float distanceRange)
        {
            if (poolEnemy == null)
                throw new ArgumentNullException(nameof(poolEnemy));
            
            if (spawnDelay < 0)
                throw new ArgumentOutOfRangeException(nameof(spawnDelay));
            
            if (maxEnemySpawnCount < 0)
                throw new ArgumentOutOfRangeException(nameof(maxEnemySpawnCount));
            
            if (distanceRange <= 0) 
                throw new ArgumentOutOfRangeException(nameof(distanceRange));

            _poolEnemy = poolEnemy;
            _spawnDelay = spawnDelay;
            _maxEnemySpawnCount = maxEnemySpawnCount;
            _distanceRange = distanceRange;
        }

        // TODO: delete for implementation spawn enemies
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
            List<Enemy> activeEnemies = _poolEnemy.ActiveItems;
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
            
            Enemy enemy = _poolEnemy.Get();
            SetPosition(enemy);
        }
        
        private void SetPosition(Enemy enemy)
        {
            float positionY = enemy.transform.position.y;
            enemy.transform.position = Random.insideUnitSphere * _distanceRange;

            enemy.transform.position = new Vector3(enemy.transform.position.x, positionY, enemy.transform.position.z);
        }
    }
}