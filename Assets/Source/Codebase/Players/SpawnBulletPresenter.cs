using System;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Players
{
    public class SpawnBulletPresenter : IDisposable
    {
        private readonly GameLoopMediator _gameLoopMediator;
        private readonly SpawnerBullet _spawnerBullet;

        public SpawnBulletPresenter(GameLoopMediator gameLoopMediator, SpawnerBullet spawnerBullet)
        {
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));
            _spawnerBullet = spawnerBullet ? spawnerBullet : throw new ArgumentNullException(nameof(spawnerBullet));

            _gameLoopMediator.GameStarted += OnSpawnBullets;
            _gameLoopMediator.GameRestarting += OnResetBulletPool;
            _gameLoopMediator.WaveCompleted += OnResetBulletPool;
        }

        public void Dispose()
        {
            _gameLoopMediator.GameStarted -= OnSpawnBullets;
            _gameLoopMediator.GameRestarting -= OnResetBulletPool;
            _gameLoopMediator.WaveCompleted += OnResetBulletPool;
        }

        private void OnSpawnBullets() => 
            _spawnerBullet.StartSpawn();

        private void OnResetBulletPool() => 
            _spawnerBullet.ResetPool();
    }
}