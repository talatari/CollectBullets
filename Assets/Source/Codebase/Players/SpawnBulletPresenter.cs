using System;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Infrastructure.Spawners;

namespace Source.Codebase.Players
{
    public class SpawnBulletPresenter : IDisposable
    {
        private readonly GameLoopMediator _gameLoopMediator;
        private readonly SpawnerBullet _spawnerBullet;

        public SpawnBulletPresenter(GameLoopMediator _gameLoopMediator, SpawnerBullet spawnerBullet)
        {
            this._gameLoopMediator = _gameLoopMediator ?? throw new ArgumentNullException(nameof(_gameLoopMediator));
            _spawnerBullet = spawnerBullet ? spawnerBullet : throw new ArgumentNullException(nameof(spawnerBullet));

            this._gameLoopMediator.GameStarted += OnSpawnBullets;
            this._gameLoopMediator.GameRestarting += OnResetBulletPool;
            this._gameLoopMediator.WaveCompleted += OnResetBulletPool;
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