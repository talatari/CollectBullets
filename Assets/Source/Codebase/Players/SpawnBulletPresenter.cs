using System;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Players
{
    public class SpawnBulletPresenter : IDisposable
    {
        private readonly GameLoopService _gameLoopService;
        private readonly SpawnerBullet _spawnerBullet;

        public SpawnBulletPresenter(GameLoopService gameLoopService, SpawnerBullet spawnerBullet)
        {
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _spawnerBullet = spawnerBullet ? spawnerBullet : throw new ArgumentNullException(nameof(spawnerBullet));

            _gameLoopService.GameStarted += OnSpawnBullets;
        }

        public void Dispose() => 
            _gameLoopService.GameStarted -= OnSpawnBullets;

        private void OnSpawnBullets() => 
            _spawnerBullet.StartSpawn();
    }
}