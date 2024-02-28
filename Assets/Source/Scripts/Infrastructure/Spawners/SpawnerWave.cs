using System;
using Source.Scripts.SO;
using UnityEngine;

namespace Source.Scripts.Infrastructure.Spawners
{
    public class SpawnerWave : IDisposable
    {
        private readonly SpawnerEnemy _spawnerEnemy;
        private int _waveNumber;
        private int _currentCount;
        private int _incrementCount;
        private float _currentDelay;
        private float _decrementDelay;
        
        public SpawnerWave(SpawnerEnemy spawnerEnemy, WaveScriptableObject waveConfig)
        {
            _spawnerEnemy = spawnerEnemy ? spawnerEnemy : throw new ArgumentNullException(nameof(spawnerEnemy));
            if (waveConfig.DefaultCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(waveConfig.DefaultCount));
            if (waveConfig.IncrementCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(waveConfig.IncrementCount));
            if (waveConfig.DefaultDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(waveConfig.DefaultDelay));
            if (waveConfig.DecrementDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(waveConfig.DecrementDelay));

            _currentCount = waveConfig.DefaultCount;
            _incrementCount = waveConfig.IncrementCount;
            _currentDelay = waveConfig.DefaultDelay;
            _decrementDelay = waveConfig.DecrementDelay;
            _spawnerEnemy.SpawnEnded += OnStopSpawn;
            _spawnerEnemy.WaveCompleted += StartSpawn;
        }
        
        public void Dispose()
        {
            _spawnerEnemy.SpawnEnded -= OnStopSpawn;
            _spawnerEnemy.WaveCompleted -= StartSpawn;
        }

        public void StartSpawn()
        {
            _waveNumber++;
            _currentCount += _incrementCount;
            _currentDelay = Mathf.Clamp(_currentDelay -= _decrementDelay, _decrementDelay, _currentDelay);
            
            _spawnerEnemy.StartSpawn(_waveNumber, _currentCount, _currentDelay);
        }
        
        private void OnStopSpawn() => 
            _spawnerEnemy.StopSpawn();
    }
}