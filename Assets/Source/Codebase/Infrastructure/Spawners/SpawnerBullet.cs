using System;
using System.Collections;
using Source.Codebase.Bullets;
using Source.Codebase.Infrastructure.Pools;
using Source.Codebase.Infrastructure.Spawners.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Codebase.Infrastructure.Spawners
{
    public class SpawnerBullet : MonoBehaviour, ISpawner
    {
        private Pool<Bullet> _poolBullet;
        private Coroutine _coroutineSpawnBullet;
        private float _spawnDelay;
        private int _maxBulletSpawnCount;
        private float _distanceRange;

        public void Init(Pool<Bullet> poolBullet, float spawnDelay, int maxBulletSpawnCount, float distanceRange)
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

        private void OnDisable() => 
            StopSpawn();

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

        public void ResetPool()
        {
            _poolBullet.ReleaseAll();
            StartSpawn();
        }

        private void Spawn()
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

        private IEnumerator SpawnBullet()
        {
            WaitForSeconds delay = new WaitForSeconds(_spawnDelay);

            while (enabled)
            {
                Spawn();

                yield return delay;
            }
        }
    }
}