using System;
using System.Collections;
using System.Collections.Generic;
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

        // TODO: delete for implementation spawn keys
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
            List<Key> activeKeys = _poolKey.ActiveItems;
            int randomIndex = Random.Range(0, activeKeys.Count);
            
            if (randomIndex < 0 || randomIndex >= activeKeys.Count)
                return;
            
            _poolKey.Release(activeKeys[randomIndex]);
        }

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

        private IEnumerator SpawnKey()
        {
            WaitForSeconds delay = new WaitForSeconds(_spawnDelay);

            while (enabled)
            {
                if (_poolKey.ActiveItems.Count >= _poolKey.StartItemCount)
                    break;
                
                Spawn();

                yield return delay;
            }
        }

        private void Spawn()
        {
            if (_poolKey.AllItemsCount >= _maxKeySpawnCount)
                return;
            
            Key key = _poolKey.Get();
            SetPosition(key);
        }
        
        private void SetPosition(Key key)
        {
            float positionX = GetRandomValue(_distanceRange, _distanceRange);
            float positionZ = GetRandomValue(_distanceRange, _distanceRange);

            key.transform.position = new Vector3(positionX, key.transform.position.y, positionZ);
        }
        
        private float GetRandomValue(float min, float max) =>
            Random.Range(-1 * min, max);
    }
}