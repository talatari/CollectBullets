using System;
using System.Collections;
using System.Collections.Generic;
using Source.Scripts.Bullets;
using Source.Scripts.Infrastructure.Bullets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Spawners
{
    public class SpawnerBullet : MonoBehaviour
    {
        private PoolBullet _poolBullet;
        private Coroutine _coroutineSpawnBullet;
        private float _spawnDelay;
        private int _maxBulletSpawnCount;

        public void Construct(PoolBullet poolBullet, float spawnDelay, int maxBulletSpawnCount)
        {
            if (poolBullet == null) 
                throw new ArgumentNullException(nameof(poolBullet));
            
            if (spawnDelay < 0) 
                throw new ArgumentOutOfRangeException(nameof(spawnDelay));
            
            if (maxBulletSpawnCount < 0) 
                throw new ArgumentOutOfRangeException(nameof(maxBulletSpawnCount));
            
            _poolBullet = poolBullet;
            _spawnDelay = spawnDelay;
            _maxBulletSpawnCount = maxBulletSpawnCount;
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
            List<Bullet> activeBullets = _poolBullet.ActiveBullets;
            int randomIndex = Random.Range(0, activeBullets.Count);
            
            if (randomIndex < 0 || randomIndex >= activeBullets.Count)
                return;
            
            _poolBullet.Release(activeBullets[randomIndex]);
        }

        public void StartSpawn()
        {
            StopSpawn();

            _coroutineSpawnBullet = StartCoroutine(SpawnBullet());
        }

        public void StopSpawn()
        {
            if (_coroutineSpawnBullet != null)
                StopCoroutine(_coroutineSpawnBullet);
        }

        private IEnumerator SpawnBullet()
        {
            WaitForSeconds delay = new WaitForSeconds(_spawnDelay);

            while (enabled)
            {
                if (_poolBullet.ActiveBullets.Count >= _poolBullet.StartBulletCount)
                    break;
                
                Spawn();

                yield return delay;
            }
        }

        private void Spawn()
        {
            if (_poolBullet.GetAmountAllBullets >= _maxBulletSpawnCount)
                return;
            
            _poolBullet.Get();
        }
    }
}