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
        private readonly ProgressService _progressService;

        public WavePresenter(
            GameLoopMediator gameLoopMediator, 
            SpawnerWave spawnerWave, 
            KeyService keyService, 
            ProgressService progressService)
        {
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));
            _spawnerWave = spawnerWave ?? throw new ArgumentNullException(nameof(spawnerWave));
            _keyService = keyService ?? throw new ArgumentNullException(nameof(keyService));
            _progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
            
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

        private void OnGameStarted()
        {
            int waveNumber = _progressService.WaveNumberCompleted;
            int amountKey = _progressService.CountKeySpawned;
            
            _spawnerWave.SetWaveNumber(waveNumber);
            _keyService.SetCountWaveCompleted(waveNumber);
            _keyService.SpawnKey(amountKey);
            
            _spawnerWave.StartSpawnWave();
        }

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