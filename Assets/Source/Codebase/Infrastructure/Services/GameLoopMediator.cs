using System;

namespace Source.Codebase.Infrastructure.Services
{
    public class GameLoopMediator : IDisposable
    {
        private UpgradeService _upgradeService;
        
        public event Action GameStarted;
        public event Action GameOver;
        public event Action GameRestarting;
        public event Action KeySpawned;
        public event Action KeyCollected;
        public event Action KeyUsed;
        public event Action<int> WaveCompleted;
        public event Action<int> WaveNumberCompletedLoaded;
        public event Action<int> CountKeySpawnedLoaded;

        public void Init(UpgradeService upgradeService)
        {
            _upgradeService = upgradeService ?? throw new ArgumentNullException(nameof(upgradeService));

            _upgradeService.UpgradeServiceInited += NotifyStartGame;
        }

        public void Dispose() => 
            _upgradeService.UpgradeServiceInited -= NotifyStartGame;

        public void NotifyGameOver() => 
            GameOver?.Invoke();

        public void NotifyRestartGame() => 
            GameRestarting?.Invoke();

        public void NotifyKeySpawned() =>
            KeySpawned?.Invoke();

        public void NotifyKeyCollected() => 
            KeyCollected?.Invoke();

        public void NotityKeyUsed() => 
            KeyUsed?.Invoke();

        public void NotifyWaveCompleted(int numberWave)
        {
            if (numberWave < 0) 
                throw new ArgumentOutOfRangeException(nameof(numberWave));

            WaveCompleted?.Invoke(numberWave);
        }

        public void NotifyWaveNumberCompletedLoaded(int waveNumberCompleted)
        {
            if (waveNumberCompleted < 0) 
                throw new ArgumentOutOfRangeException(nameof(waveNumberCompleted));
            
            WaveNumberCompletedLoaded?.Invoke(waveNumberCompleted);
        }

        public void NotifyCountKeySpawnedLoaded(int countKeySpawned)
        {
            if (countKeySpawned < 0) 
                throw new ArgumentOutOfRangeException(nameof(countKeySpawned));
            
            CountKeySpawnedLoaded?.Invoke(countKeySpawned);
        }

        private void NotifyStartGame() => 
            GameStarted?.Invoke();
    }
}