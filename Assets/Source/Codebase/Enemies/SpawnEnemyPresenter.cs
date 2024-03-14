using System;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Enemies
{
    public class SpawnEnemyPresenter : IDisposable
    {
        private readonly GameLoopMediator _gameLoopMediator;
        private readonly SpawnerEnemy _spawnerEnemy;

        public SpawnEnemyPresenter(GameLoopMediator _gameLoopMediator, SpawnerEnemy spawnerEnemy)
        {
            this._gameLoopMediator = _gameLoopMediator ?? throw new ArgumentNullException(nameof(_gameLoopMediator));
            _spawnerEnemy = spawnerEnemy ? spawnerEnemy : throw new ArgumentNullException(nameof(spawnerEnemy));
            
            this._gameLoopMediator.GameRestarting += OnGameRestarting;
        }

        public void Dispose() => 
            _gameLoopMediator.GameRestarting -= OnGameRestarting;

        private void OnGameRestarting() => 
            _spawnerEnemy.ResetPool();
    }
}