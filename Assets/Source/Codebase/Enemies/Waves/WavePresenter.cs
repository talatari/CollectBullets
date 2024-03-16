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

        public WavePresenter(GameLoopMediator gameLoopMediator, SpawnerWave spawnerWave, KeyService keyService)
        {
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));
            _spawnerWave = spawnerWave ?? throw new ArgumentNullException(nameof(spawnerWave));
            _keyService = keyService ?? throw new ArgumentNullException(nameof(keyService));
            
            _gameLoopMediator.GameStarted += OnGameStarted;
            _gameLoopMediator.GameRestarting += OnGameRestarting;
            _spawnerWave.WaveNumberCompleted += OnWaveNumberCompleted;
        }

        public void Dispose()
        {
            _gameLoopMediator.GameStarted -= OnGameStarted;
            _gameLoopMediator.GameRestarting -= OnGameRestarting;
            _spawnerWave.WaveNumberCompleted -= OnWaveNumberCompleted;
        }

        private void OnGameStarted() => 
            _spawnerWave.StartSpawnWave();

        private void OnGameRestarting()
        {
            _spawnerWave.RestartWave();
            _keyService.ResetKeyPool();
        }

        private void OnWaveNumberCompleted(int numberWave)
        {
            _gameLoopMediator.NotifyWaveCompleted(numberWave);
            _keyService.SpawnKey();
        }
    }
}