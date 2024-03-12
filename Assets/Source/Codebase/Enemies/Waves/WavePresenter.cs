using System;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Enemies.Waves
{
    public class WavePresenter : IDisposable
    {
        private readonly GameLoopService _gameLoopService;
        private readonly SpawnerWave _spawnerWave;
        private readonly KeyService _keyService;

        public WavePresenter(GameLoopService gameLoopService, SpawnerWave spawnerWave, KeyService keyService)
        {
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _spawnerWave = spawnerWave ?? throw new ArgumentNullException(nameof(spawnerWave));
            _keyService = keyService ?? throw new ArgumentNullException(nameof(keyService));
            
            _gameLoopService.GameStarted += OnGameStarted;
            _gameLoopService.GameRestarting += OnGameRestarting;
            _spawnerWave.Completed += OnCompleted;
        }

        public void Dispose()
        {
            _gameLoopService.GameStarted -= OnGameStarted;
            _gameLoopService.GameRestarting -= OnGameRestarting;
            _spawnerWave.Completed -= OnCompleted;
        }

        private void OnGameStarted() => 
            _spawnerWave.StartSpawnWave();

        private void OnGameRestarting()
        {
            _spawnerWave.RestartWave();
            _keyService.ResetKeyPool();
        }
        
        private void OnCompleted()
        {
            _gameLoopService.NotifyWaveCompleted();
            _keyService.NotifyWaveCompleted();
        }
    }
}