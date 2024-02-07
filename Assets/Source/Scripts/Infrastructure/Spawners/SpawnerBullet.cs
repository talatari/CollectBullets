using System;
using System.Collections;
using System.Collections.Generic;
using Source.Scripts.Bullets;
using Source.Scripts.Infrastructure.Pools;
using Source.Scripts.Infrastructure.Spawners.Intefaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Infrastructure.Spawners
{
    public class SpawnerBullet : MonoBehaviour, ISpawner
    {
        private Pool<Bullet> _poolBullet;
        private Coroutine _coroutineSpawnBullet;
        private float _spawnDelay;
        private int _maxBulletSpawnCount;
        private float _distanceRange;

        public void Construct(Pool<Bullet> poolBullet, float spawnDelay, int maxBulletSpawnCount, float distanceRange)
        {
            if (poolBullet == null) 
                throw new ArgumentNullException(nameof(poolBullet));
            
            if (spawnDelay < 0) 
                throw new ArgumentOutOfRangeException(nameof(spawnDelay));
            
            if (maxBulletSpawnCount < 0) 
                throw new ArgumentOutOfRangeException(nameof(maxBulletSpawnCount));
            
            if (distanceRange <= 0) 
                throw new ArgumentOutOfRangeException(nameof(distanceRange));

            _poolBullet = poolBullet;
            _spawnDelay = spawnDelay;
            _maxBulletSpawnCount = maxBulletSpawnCount;
            _distanceRange = distanceRange;
        }

        // TODO: delete for implementation spawn bullets
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Spawn();
            
            if (Input.GetKeyDown(KeyCode.R))
                Release();
        }

        private void OnDisable() => 
            StopSpawn();

        private void Release()
        {
            List<Bullet> activeBullets = _poolBullet.ActiveItems;
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
                if (_poolBullet.ActiveItems.Count >= _poolBullet.StartItemCount)
                    break;
                
                Spawn();

                yield return delay;
            }
        }

        public void Spawn()
        {
            if (_poolBullet.AllItemsCount >= _maxBulletSpawnCount)
                return;
            
            Bullet bullet = _poolBullet.Get();
            SetPosition(bullet);
        }

        private void SetPosition(Bullet bullet)
        {
            bullet.transform.position = Random.insideUnitSphere * _distanceRange;

            if (bullet.transform.position.y < 0)
            {
                float newPositionY = bullet.transform.position.y * -1;

                bullet.transform.position =
                    new Vector3(bullet.transform.position.x, newPositionY, bullet.transform.position.z);
            }
        }
    }
}