using System;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Players;

namespace Source.Codebase.GameOver
{
    public class GameOverPresenter : IDisposable
    {
        private readonly GameLoopMediator _gameLoopMediator;
        private readonly Player _player;

        public GameOverPresenter(GameLoopMediator gameLoopMediator, Player player)
        {
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));
            _player = player ? player : throw new ArgumentNullException(nameof(player));

            _gameLoopMediator.GameRestarting += OnGameRestarting;
            _player.Died += OnDied;
        }

        public void Dispose()
        {
            _gameLoopMediator.GameRestarting -= OnGameRestarting;
            _player.Died -= OnDied;
        }

        private void OnGameRestarting() => 
            _player.Restart();

        private void OnDied() => 
            _gameLoopMediator.NotifyGameOver();
    }
}