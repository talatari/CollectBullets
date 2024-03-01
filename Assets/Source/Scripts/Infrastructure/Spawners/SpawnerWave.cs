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
        private int _incrementPercent;
        private float _currentDelay;
        private float _decrementDelay;
        private float _delayBetweenWaves;
        private float _hundredPercent = 100f;
        
        public SpawnerWave(SpawnerEnemy spawnerEnemy, WaveScriptableObject waveConfig)
        {
            _spawnerEnemy = spawnerEnemy ? spawnerEnemy : throw new ArgumentNullException(nameof(spawnerEnemy));
            if (waveConfig.DefaultCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(waveConfig.DefaultCount));
            if (waveConfig.IncrementPercent <= 0)
                throw new ArgumentOutOfRangeException(nameof(waveConfig.IncrementPercent));
            if (waveConfig.DefaultDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(waveConfig.DefaultDelay));
            if (waveConfig.DecrementDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(waveConfig.DecrementDelay));
            if (waveConfig.DelayBetweenWaves <= 0)
                throw new ArgumentOutOfRangeException(nameof(waveConfig.DelayBetweenWaves));

            _currentCount = waveConfig.DefaultCount;
            _incrementPercent = waveConfig.IncrementPercent;
            _currentDelay = waveConfig.DefaultDelay;
            _decrementDelay = waveConfig.DecrementDelay;
            _delayBetweenWaves = waveConfig.DelayBetweenWaves;
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
            float increment = (_currentCount / _hundredPercent) * _incrementPercent;
            _currentCount += (int) increment;
            _currentDelay = Mathf.Clamp(_currentDelay -= _decrementDelay, _decrementDelay, _currentDelay);
            _spawnerEnemy.StartSpawn(_waveNumber, _currentCount, _currentDelay, _delayBetweenWaves);
        }
        
        private void OnStopSpawn() => 
            _spawnerEnemy.StopSpawn();
    }
}