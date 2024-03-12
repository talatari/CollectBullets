using System;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Infrastructure.Services
{
    public class KeyService
    {
        private SpawnerKey _spawnerKey;
        private int _spawnInterval;

        public void Init(SpawnerKey spawnerKey, int spawnInterval)
        {
            _spawnerKey = spawnerKey ? spawnerKey : throw new ArgumentNullException(nameof(spawnerKey));
            if (spawnInterval <= 0) 
                throw new ArgumentOutOfRangeException(nameof(spawnInterval));
            
            _spawnInterval = spawnInterval;
        }

        public void NotifyWaveCompleted() => 
            SpawnKey();

        public void ResetKeyPool() =>
            _spawnerKey.ResetPool();

        private void SpawnKey()
        {
            _spawnInterval++;

            if (_spawnInterval == 2)
            {
                _spawnerKey.Spawn();
                _spawnInterval = 0;
            }
        }
    }
}