using System;
using System.Collections;
using Source.Scripts.Infrastructure.Pools;
using Source.Scripts.Infrastructure.Spawners.Intefaces;
using Source.Scripts.Keys;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Infrastructure.Spawners
{
    public class SpawnerKey : MonoBehaviour, ISpawner
    {
        private Pool<Key> _poolKey;
        private Coroutine _coroutineSpawnKey;
        private float _spawnDelay;
        private float _distanceRange;
        private int _maxKeySpawnCount;
        private int _spawnedCount;

        public void Construct(Pool<Key> poolKey, float spawnDelay, float distanceRange, int maxKeySpawnCount)
        {
            if (poolKey == null) 
                throw new ArgumentNullException(nameof(poolKey));
            
            if (spawnDelay < 0) 
                throw new ArgumentOutOfRangeException(nameof(spawnDelay));
            
            if (distanceRange <= 0)
                throw new ArgumentOutOfRangeException(nameof(distanceRange));
            
            if (maxKeySpawnCount < 0) 
                throw new ArgumentOutOfRangeException(nameof(maxKeySpawnCount));

            _poolKey = poolKey;
            _spawnDelay = spawnDelay;
            _distanceRange = distanceRange;
            _maxKeySpawnCount = maxKeySpawnCount;
        }

        private void OnDisable() => 
            StopSpawn();

        public void StartSpawn()
        {
            StopSpawn();

            _coroutineSpawnKey = StartCoroutine(SpawnKey());
        }

        public void StopSpawn()
        {
            if (_coroutineSpawnKey != null)
                StopCoroutine(_coroutineSpawnKey);
        }

        private void Spawn()
        {
            if (_poolKey.AllItemsCount >= _maxKeySpawnCount)
                return;
            
            Key key = _poolKey.Get();
            SetPosition(key);
            _spawnedCount++;
        }

        private void SetPosition(Key key)
        {
            float positionX = GetRandomValue(_distanceRange, _distanceRange);
            float positionZ = GetRandomValue(_distanceRange, _distanceRange);

            key.transform.position = new Vector3(positionX, key.transform.position.y, positionZ);
        }

        private float GetRandomValue(float min, float max) =>
            Random.Range(-1 * min, max);

        private IEnumerator SpawnKey()
        {
            WaitForSeconds delay = new WaitForSeconds(_spawnDelay);

            while (enabled)
            {
                if (_spawnedCount >= _poolKey.StartItemCount)
                    break;
                
                Spawn();

                yield return delay;
            }
        }
    }
}