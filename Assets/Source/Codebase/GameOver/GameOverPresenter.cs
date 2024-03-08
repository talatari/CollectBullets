using System;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Players;

namespace Source.Codebase.GameOver
{
    public class GameOverPresenter : IDisposable
    {
        private readonly GameLoopService _gameLoopService;
        private readonly Player _player;

        public GameOverPresenter(GameLoopService gameLoopService, Player player)
        {
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _player = player ? player : throw new ArgumentNullException(nameof(player));

            _gameLoopService.GameRestarting += OnGameRestarting;
            _player.Died += OnDied;
        }

        public void Dispose() => 
            _player.Died -= OnDied;

        private void OnGameRestarting() => 
            _player.Restart();

        private void OnDied() => 
            _gameLoopService.NotifyGameOver();
    }
}