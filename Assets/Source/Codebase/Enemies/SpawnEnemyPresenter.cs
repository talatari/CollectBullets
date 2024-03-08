using System;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Enemies
{
    public class SpawnEnemyPresenter : IDisposable
    {
        private readonly GameLoopService _gameLoopService;
        private readonly SpawnerEnemy _spawnerEnemy;

        public SpawnEnemyPresenter(GameLoopService gameLoopService, SpawnerEnemy spawnerEnemy)
        {
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _spawnerEnemy = spawnerEnemy ? spawnerEnemy : throw new ArgumentNullException(nameof(spawnerEnemy));
            
            _gameLoopService.GameRestarted += OnGameRestarted;
        }

        public void Dispose() => 
            _gameLoopService.GameRestarted -= OnGameRestarted;

        private void OnGameRestarted() => 
            _spawnerEnemy.ResetPool();
    }
}