using System;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Enemies
{
    public class SpawnEnemyPresenter : IDisposable
    {
        private readonly GameLoopMediator _gameLoopMediator;
        private readonly SpawnerEnemy _spawnerEnemy;

        public SpawnEnemyPresenter(GameLoopMediator gameLoopMediator, SpawnerEnemy spawnerEnemy)
        {
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));
            _spawnerEnemy = spawnerEnemy ? spawnerEnemy : throw new ArgumentNullException(nameof(spawnerEnemy));
            
            _gameLoopMediator.GameRestarting += OnGameRestarting;
        }

        public void Dispose() => 
            _gameLoopMediator.GameRestarting -= OnGameRestarting;

        private void OnGameRestarting() => 
            _spawnerEnemy.ResetPool();
    }
}