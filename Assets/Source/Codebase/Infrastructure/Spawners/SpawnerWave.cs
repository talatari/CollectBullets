using System;
using Source.Codebase.SO;
using UnityEngine;

namespace Source.Codebase.Infrastructure.Spawners
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
        private float _miltiplyer = 100f;
        private readonly int _defaultCount;
        private readonly float _defaultDelay;

        public SpawnerWave(SpawnerEnemy spawnerEnemy, WaveScriptableObject waveConfig)
        {
            _spawnerEnemy = spawnerEnemy ? spawnerEnemy : throw new ArgumentNullException(nameof(spawnerEnemy));
            
            if (waveConfig == null)
                throw new ArgumentNullException(nameof(waveConfig));
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

            // TODO: QUESTIONS: не считается действия ниже логикой? стоит вынести в Init?
            _defaultCount = waveConfig.DefaultCount;
            _currentCount = _defaultCount;
            _incrementPercent = waveConfig.IncrementPercent;
            _defaultDelay = waveConfig.DefaultDelay / _miltiplyer; // TODO: QUESTIONS: появилась такая грязюка из-за того, чтобы в SO не было float
            _currentDelay = _defaultDelay;
            _decrementDelay = waveConfig.DecrementDelay / _miltiplyer; // TODO: QUESTIONS: как можно исправить?
            _delayBetweenWaves = waveConfig.DelayBetweenWaves;

            _spawnerEnemy.Completed += OnSpawn;
            _spawnerEnemy.SpawnEnded += OnStopSpawn;
        }

        public event Action<int> WaveNumberCompleted;

        public void Dispose()
        {
            _spawnerEnemy.Completed -= OnSpawn;
            _spawnerEnemy.SpawnEnded -= OnStopSpawn;
        }

        public void StartSpawnWave()
        {
            _waveNumber++;
            float increment = (_currentCount / _hundredPercent) * _incrementPercent;
            _currentCount += (int) increment;
            _currentDelay = Mathf.Clamp(_currentDelay -= _decrementDelay, _decrementDelay, _currentDelay);
            _spawnerEnemy.StartSpawn(_waveNumber, _currentCount, _currentDelay, _delayBetweenWaves);
        }

        public void RestartWave()
        {
            _waveNumber = 0;
            _currentCount = _defaultCount;
            _currentDelay = _defaultDelay;

            StartSpawnWave();
        }

        public void SetWaveNumber(int waveNumber)
        {
            if (waveNumber < 0) 
                throw new ArgumentOutOfRangeException(nameof(waveNumber));
            
            _waveNumber = waveNumber;
        }

        private void OnSpawn()
        {
            WaveNumberCompleted?.Invoke(_waveNumber);
            StartSpawnWave();
        }

        private void OnStopSpawn() => 
            _spawnerEnemy.StopSpawn();
    }
}