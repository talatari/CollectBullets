using System;

namespace Source.Scripts.Infrastructure.Spawners
{
    public class SpawnerWave : IDisposable
    {
        private SpawnerEnemy _spawnerEnemy;
        private int _waveNumber = 0;

        public void Init(SpawnerEnemy spawnerEnemy)
        {
            _spawnerEnemy = spawnerEnemy ? spawnerEnemy : throw new ArgumentNullException(nameof(spawnerEnemy));
            _spawnerEnemy.SpawnEnded += OnStopSpawn;
        }

        public void Dispose() => 
            _spawnerEnemy.SpawnEnded -= OnStopSpawn;

        public void StartSpawn()
        {
            _waveNumber++;
            _spawnerEnemy.StartSpawn(_waveNumber, _spawnerEnemy.SpawnDelay);
        }

        private void OnStopSpawn() => 
            _spawnerEnemy.StopSpawn();
    }
}