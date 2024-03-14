using System;

namespace Source.Codebase.Infrastructure.Services
{
    public class GameLoopMediator
    {
        public event Action GameStarted;
        public event Action WaveCompleted;
        public event Action GameOver;
        public event Action GameRestarting;
        public event Action KeyCollected;
        public event Action KeyUsed;

        public void NotifyStartGame() => 
            GameStarted?.Invoke();

        public void NotifyWaveCompleted() => 
            WaveCompleted?.Invoke();

        public void NotifyGameOver() => 
            GameOver?.Invoke();

        public void NotifyRestartGame() => 
            GameRestarting?.Invoke();

        public void NotifyKeyCollected() => 
            KeyCollected?.Invoke();

        public void NotityKeyUsed() => 
            KeyUsed?.Invoke();
    }
}