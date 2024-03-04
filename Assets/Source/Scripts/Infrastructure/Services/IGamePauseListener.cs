namespace Source.Scripts.Infrastructure.Services
{
    public interface IGamePauseListener
    {
        void OnGamePaused();
        void OnGameResumed();
    }
}