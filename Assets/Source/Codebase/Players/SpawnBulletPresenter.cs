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
            _gameLoopService.GameRestarting += OnResetBulletPool;
            _gameLoopService.WaveCompleted += OnResetBulletPool;
        }

        public void Dispose()
        {
            _gameLoopService.GameStarted -= OnSpawnBullets;
            _gameLoopService.GameRestarting -= OnResetBulletPool;
            _gameLoopService.WaveCompleted += OnResetBulletPool;
        }

        private void OnSpawnBullets() => 
            _spawnerBullet.StartSpawn();

        private void OnResetBulletPool() => 
            _spawnerBullet.ResetPool();
    }
}