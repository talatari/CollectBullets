using System;
using Source.Codebase.Infrastructure.Services;

namespace Source.Codebase.Infrastructure.Spawners
{
    public class WavePresenter : IDisposable
    {
        private readonly GameLoopService _gameLoopService;
        private readonly SpawnerWave _spawnerWave;

        public WavePresenter(GameLoopService gameLoopService, SpawnerWave spawnerWave)
        {
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _spawnerWave = spawnerWave ?? throw new ArgumentNullException(nameof(spawnerWave));
            
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

        private void OnGameRestarting() => 
            _spawnerWave.RestartWave();

        private void OnCompleted() => 
            _gameLoopService.NotifyWaveCompleted();
    }
}