using System;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Players;

namespace Source.Codebase.GameOver
{
    public class GameOverPresenter : IDisposable
    {
        private readonly GameLoopMediator _gameLoopMediator;
        private readonly Player _player;

        public GameOverPresenter(GameLoopMediator _gameLoopMediator, Player player)
        {
            this._gameLoopMediator = _gameLoopMediator ?? throw new ArgumentNullException(nameof(_gameLoopMediator));
            _player = player ? player : throw new ArgumentNullException(nameof(player));

            this._gameLoopMediator.GameRestarting += OnGameRestarting;
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