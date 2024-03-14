using System;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Infrastructure.Services
{
    public class KeyService : IDisposable
    {
        private SpawnerKey _spawnerKey;
        private int _spawnInterval;
        private int _countWaveCompleted;
        private GameLoopService _gameLoopService;

        public void Init(SpawnerKey spawnerKey, int spawnInterval, GameLoopService gameLoopService)
        {
            _spawnerKey = spawnerKey ? spawnerKey : throw new ArgumentNullException(nameof(spawnerKey));
            if (spawnInterval <= 0) 
                throw new ArgumentOutOfRangeException(nameof(spawnInterval));
            _gameLoopService = gameLoopService;
            
            _spawnInterval = spawnInterval;
            _gameLoopService.GameOver += OnResetCountWave;
        }

        public void Dispose() => 
            _gameLoopService.GameOver -= OnResetCountWave;

        public void NotifyWaveCompleted() => 
            SpawnKey();

        public void ResetKeyPool() =>
            _spawnerKey.ResetPool();

        public void DropKey() => 
            _spawnerKey.DropKey();

        private void SpawnKey()
        {
            _countWaveCompleted++;

            if (_countWaveCompleted == _spawnInterval)
            {
                _spawnerKey.Spawn();
                _countWaveCompleted = 0;
            }
        }

        private void OnResetCountWave() => 
            _countWaveCompleted = 0;
    }
}