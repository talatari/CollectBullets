using System;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Infrastructure.Services
{
    public class KeyService : IDisposable
    {
        private SpawnerKey _spawnerKey;
        private int _spawnInterval;
        private int _countWaveCompleted;
        private GameLoopMediator _gameLoopMediator;
        private bool _isKeyCollected;

        public void Init(SpawnerKey spawnerKey, int spawnInterval, GameLoopMediator gameLoopMediator)
        {
            _spawnerKey = spawnerKey ? spawnerKey : throw new ArgumentNullException(nameof(spawnerKey));
            if (spawnInterval <= 0) 
                throw new ArgumentOutOfRangeException(nameof(spawnInterval));
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));
            
            _spawnInterval = spawnInterval;
            _gameLoopMediator.GameOver += OnResetCountWave;
            _gameLoopMediator.KeyCollected += OnKeyCollected;
            _gameLoopMediator.KeyUsed += OnKeyUsed;
        }

        public void Dispose()
        {
            _gameLoopMediator.GameOver -= OnResetCountWave;
            _gameLoopMediator.KeyCollected -= OnKeyCollected;
            _gameLoopMediator.KeyUsed -= OnKeyUsed;
        }

        public void ResetKeyPool() =>
            _spawnerKey.ResetPool();

        // public void DropKey() => 
        //     _spawnerKey.DropKey();

        public void SpawnKey()
        {
            _countWaveCompleted++;

            if (_countWaveCompleted < _spawnInterval)
                return;
            
            if (_spawnerKey.CanSpawn(_isKeyCollected) == false)
                return;
            
            _spawnerKey.Spawn();
            _countWaveCompleted = 0;
        }

        private void OnKeyCollected() => 
            _isKeyCollected = true;

        private void OnKeyUsed() => 
            _isKeyCollected = false;

        private void OnResetCountWave()
        {
            _countWaveCompleted = 0;
            _isKeyCollected = false;
        }
    }
}