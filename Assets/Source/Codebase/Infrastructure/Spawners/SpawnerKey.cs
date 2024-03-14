using System;
using Source.Codebase.Infrastructure.Pools;
using Source.Codebase.Keys;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Codebase.Infrastructure.Spawners
{
    public class SpawnerKey : MonoBehaviour
    {
        private Pool<Key> _poolKey;
        private int _maxKeySpawnCount;
        private float _distanceRange;

        public int MaxKeySpawnCount => _maxKeySpawnCount;

        public void Init(Pool<Key> poolKey, int maxKeySpawnCount, float distanceRange)
        {
            if (poolKey == null) 
                throw new ArgumentNullException(nameof(poolKey));
            
            if (maxKeySpawnCount <= 0) 
                throw new ArgumentOutOfRangeException(nameof(maxKeySpawnCount));
            
            if (distanceRange <= 0) 
                throw new ArgumentOutOfRangeException(nameof(distanceRange));
            
            _poolKey = poolKey;
            _maxKeySpawnCount = maxKeySpawnCount;
            _distanceRange = distanceRange;
        }

        public bool CanSpawn(bool isKeyCollected) =>
            isKeyCollected
                ? _poolKey.ActiveItems.Count < _maxKeySpawnCount - 1
                : _poolKey.ActiveItems.Count < _maxKeySpawnCount;

        public void Spawn()
        {
            if (_poolKey.ActiveItems.Count == _maxKeySpawnCount) 
                return;
            
            // if (_poolKey.IsKeyCollected && _poolKey.ActiveItems.Count == _maxKeySpawnCount - 1)
            //     return;
            
            Key key = _poolKey.Get();
            SetPosition(key);
            
            print("Key spawned");
        }
        
        public void ResetPool() => 
            _poolKey.ReleaseAll();

        // public void DropKey() => 
        //     _poolKey.DropKey();

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