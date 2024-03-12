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
        private Coroutine _coroutineSpawnKey;
        private float _distanceRange;

        public void Init(Pool<Key> poolKey, float distanceRange)
        {
            if (poolKey == null) 
                throw new ArgumentNullException(nameof(poolKey));
            
            if (distanceRange <= 0)
                throw new ArgumentOutOfRangeException(nameof(distanceRange));
            
            _poolKey = poolKey;
            _distanceRange = distanceRange;
        }

        public void Spawn()
        {
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