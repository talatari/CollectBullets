using System;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Enemies.Waves
{
    public class WavePresenter : IDisposable
    {
        private readonly GameLoopMediator _gameLoopMediator;
        private readonly SpawnerWave _spawnerWave;
        private readonly KeyService _keyService;

        public WavePresenter(GameLoopMediator _gameLoopMediator, SpawnerWave spawnerWave, KeyService keyService)
        {
            this._gameLoopMediator = _gameLoopMediator ?? throw new ArgumentNullException(nameof(_gameLoopMediator));
            _spawnerWave = spawnerWave ?? throw new ArgumentNullException(nameof(spawnerWave));
            _keyService = keyService ?? throw new ArgumentNullException(nameof(keyService));
            
            this._gameLoopMediator.GameStarted += OnGameStarted;
            this._gameLoopMediator.GameRestarting += OnGameRestarting;
            _spawnerWave.Completed += OnCompleted;
        }

        public void Dispose()
        {
            _gameLoopMediator.GameStarted -= OnGameStarted;
            _gameLoopMediator.GameRestarting -= OnGameRestarting;
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
            _gameLoopMediator.NotifyWaveCompleted();
            _keyService.SpawnKey();
        }
    }
}