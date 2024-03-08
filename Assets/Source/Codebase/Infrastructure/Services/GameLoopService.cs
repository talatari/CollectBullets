using System;

namespace Source.Codebase.Infrastructure.Services
{
    public class GameLoopService
    {
        public event Action GameStarted;
        public event Action WaveCompleted;
        public event Action GameOver;
        public event Action GameRestarting;

        public void StartGame()
        {
            GameStarted?.Invoke();
        }

        public void NotifyWaveCompleted()
        {
            WaveCompleted?.Invoke();
        }

        public void NotifyGameOver()
        {
            GameOver?.Invoke();
        }

        public void NotifyRestartGame()
        {
            GameRestarting?.Invoke();
        }
    }
}