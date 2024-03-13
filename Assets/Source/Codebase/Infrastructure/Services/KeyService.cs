using System;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Infrastructure.Services
{
    public class KeyService
    {
        private SpawnerKey _spawnerKey;
        private int _spawnInterval;
        private int _countWaveCompleted;

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
            _countWaveCompleted++;

            if (_countWaveCompleted == _spawnInterval)
            {
                _spawnerKey.Spawn();
                _countWaveCompleted = 0;
            }
        }
    }
}