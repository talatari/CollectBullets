using System;
using System.Collections.Generic;
using Source.Scripts.Enemies;
using Source.Scripts.Infrastructure.Factories;
using Source.Scripts.Infrastructure.Pools;
using Source.Scripts.Infrastructure.Providers;
using Source.Scripts.Infrastructure.SaveLoadData;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Infrastructure.Spawners;
using Source.Scripts.Keys;
using Source.Scripts.Players;
using Source.Scripts.Players.PlayerModels;
using Source.Scripts.SO;
using Source.Scripts.Upgrades;
using UnityEngine;

namespace Source.Scripts.Infrastructure
{
    public class CompositionRoot : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Player _player;
        [SerializeField] private PlayerScriptableObject _playerConfig;
        [SerializeField] private UpgradeSriptableObject[] _upgradeConfigs;
        [SerializeField] private UpgradePresenter _upgradePresenter;

        [Header("Enemies")]
        [SerializeField] private List<Enemy> _enemyPrefabs;
        [SerializeField] private WaveScriptableObject _waveScriptableObject;
        [SerializeField] private int _maxEnemySpawnCount = 100;
        [SerializeField] private Transform _enemiesParent;

        [Header("Bullets")]
        [SerializeField] private int _startBulletCount = 20;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _maxBulletSpawnCount = 100;
        [SerializeField] private float _spawnBulletDelay = 1f;
        [SerializeField] private Transform _bulletsParent;

        [Header("Others")]
        [SerializeField] private float _distanceRange = 30f;
        [SerializeField] private Key _keyPrefab;

        private Stats _stats;
        private SaveLoadService _saveLoadService = new();
        private UpgradeService _upgradeService;
        private SpawnerEnemy _spawnerEnemy;
        private SpawnerWave _spawnerWave;
        private SpawnerBullet _spawnerBullet;
        private SpawnerKey _spawnerKey;
        private List<UpgradeModel> _upgradeModels;
        private UpgradeHandler _upgradeHandler;

        private void Start()
        {
            if (_player == null)
                throw new ArgumentNullException(nameof(_player));
            
            if (_enemyPrefabs == null)
                throw new Exception($"{nameof(_enemyPrefabs)} not found.");
            
            if (_bulletPrefab == null)
                throw new Exception($"{nameof(_bulletPrefab)} not found.");
            
            if (_keyPrefab == null)
                throw new Exception($"{nameof(_keyPrefab)} not found.");
            
            if (_waveScriptableObject == null)
                throw new ArgumentNullException(nameof(_waveScriptableObject));

            FactoryDefaultStats factoryDefaultStats = new(_playerConfig);
            _stats = factoryDefaultStats.Create();

            FactoryUpgradeModels factoryUpgradeModels = new(_upgradeConfigs);
            _upgradeModels = factoryUpgradeModels.Create();

            _upgradeHandler = new UpgradeHandler(_stats, _upgradeModels);
            _player.Init(_stats, _upgradeHandler);
            
            _upgradeService = new UpgradeService(_saveLoadService, _upgradeModels);
            _upgradePresenter.Init(_stats, _upgradeService);
            
            TargetProvider targetProvider = new TargetProvider();
            targetProvider.SetTarget(_player.transform);
            
            FactoryEnemy factoryEnemy = new FactoryEnemy(_enemyPrefabs, _enemiesParent, targetProvider);
            Pool<Enemy> poolEnemy = new Pool<Enemy>(factoryEnemy, _waveScriptableObject.DefaultCount);
            poolEnemy.Init();
            
            _spawnerEnemy = gameObject.AddComponent<SpawnerEnemy>();
            _spawnerEnemy.Init(poolEnemy, _maxEnemySpawnCount, _distanceRange);
            _spawnerWave = new SpawnerWave(_spawnerEnemy, _waveScriptableObject);
            _spawnerWave.StartSpawn();
            
            FactoryBullet factoryBullet = new FactoryBullet(_bulletPrefab, _bulletsParent);
            Pool<Bullet> poolBullet = new Pool<Bullet>(factoryBullet, _startBulletCount);
            poolBullet.Init();
            
            _spawnerBullet = gameObject.AddComponent<SpawnerBullet>();
            _spawnerBullet.Init(poolBullet, _spawnBulletDelay, _maxBulletSpawnCount, _distanceRange);
            _spawnerBullet.StartSpawn();
        }

        private void OnDestroy()
        {
            _spawnerWave.Dispose();
        }
    }
}