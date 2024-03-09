using System;
using System.Collections.Generic;
using Source.Codebase.Enemies;
using Source.Codebase.GameOver;
using Source.Codebase.Infrastructure.Factories;
using Source.Codebase.Infrastructure.Pools;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Infrastructure.Spawners;
using Source.Codebase.Keys;
using Source.Codebase.Players;
using Source.Codebase.Players.PlayerModels;
using Source.Codebase.SO;
using Source.Codebase.Upgrades;
using UnityEngine;

namespace Source.Codebase.Infrastructure
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
        [SerializeField] private WaveScriptableObject _waveConfig;
        [SerializeField] private int _maxEnemySpawnCount;
        [SerializeField] private Transform _container;
        
        [Header("Bullets")]
        [SerializeField] private int _startBulletCount;
        [SerializeField] private int _maxBulletSpawnCount;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float _spawnBulletDelay;
        [SerializeField] private Transform _bulletsParent;
        
        [Header("Others")]
        [SerializeField] private float _distanceRange;
        [SerializeField] private Key _keyPrefab;
        [SerializeField] private RestartGameView _restartView;

        private FactoryDefaultStats _factoryDefaultStats;
        private Stats _stats;
        private List<UpgradeModel> _upgradeModels;
        private UpgradeHandler _upgradeHandler;
        private GamePauseService _gamePauseService;
        private GameLoopService _gameLoopService;
        private SaveLoadService _saveLoadService;
        private UpgradeService _upgradeService;
        private TargetProvider _targetProvider;
        private SpawnEnemyPresenter _spawnEnemyPresenter;
        private SpawnBulletPresenter _spawnBulletPresenter;
        private Pool<Enemy> _poolEnemy;
        private SpawnerEnemy _spawnerEnemy;
        private SpawnerWave _spawnerWave;
        private SpawnerBullet _spawnerBullet;
        private GameOverPresenter _gameOverPresenter;
        private RestartGamePresenter _restartPresenter;
        private WavePresenter _wavePresenter;

        private void Start()
        {
            CheckSerializedFields();

            CreateModels();
            
            CreateServices();

            InitTargetProvider();

            InitSpawners();

            InitPresenters();
            
            _gameLoopService.StartGame();
        }
        
        private void CheckSerializedFields()
        {
            if (_player == null)
                throw new ArgumentNullException(nameof(_player));
            
            if (_enemyPrefabs == null)
                throw new Exception($"{nameof(_enemyPrefabs)} not found.");
            
            if (_waveConfig == null)
                throw new ArgumentNullException(nameof(_waveConfig));
            
            if (_bulletPrefab == null)
                throw new Exception($"{nameof(_bulletPrefab)} not found.");
            
            if (_keyPrefab == null)
                throw new Exception($"{nameof(_keyPrefab)} not found.");
            
            if (_restartView == null)
                throw new ArgumentNullException(nameof(_restartView));
        }
        
        private void CreateModels()
        {
            CreateStats();

            CreateUpgradeModels();

            _upgradeHandler = new UpgradeHandler(_stats, _upgradeModels);
        }
        
        private void CreateStats()
        {
            _factoryDefaultStats = new(_playerConfig);
            _stats = _factoryDefaultStats.Create();
        }
        
        private void CreateUpgradeModels()
        {
            FactoryUpgradeModels factoryUpgradeModels = new(_upgradeConfigs);
            _upgradeModels = factoryUpgradeModels.Create();
        }
        
        private void CreateServices()
        {
            MultiCallHandler multiCallHandler = new MultiCallHandler();
            _gamePauseService = new GamePauseService(multiCallHandler);
            _gameLoopService = new GameLoopService();
            _saveLoadService = new SaveLoadService();
            _upgradeService = new UpgradeService(_saveLoadService, _upgradeModels);
        }
        
        private void InitTargetProvider()
        {
            _targetProvider = new TargetProvider();
            _targetProvider.SetTarget(_player.transform);
        }
        
        private void InitSpawners()
        {
            FactoryEnemy factoryEnemy = new FactoryEnemy(_enemyPrefabs, _container, _targetProvider, _stats.CommonStats);
            _poolEnemy = new Pool<Enemy>(factoryEnemy, _waveConfig.DefaultCount);
            _poolEnemy.Init();
            
            _spawnerEnemy = gameObject.AddComponent<SpawnerEnemy>();
            _spawnerEnemy.Init(_poolEnemy, _maxEnemySpawnCount, _distanceRange);
            _spawnerWave = new SpawnerWave(_spawnerEnemy, _waveConfig);
            
            FactoryBullet factoryBullet = new FactoryBullet(_bulletPrefab, _bulletsParent);
            Pool<Bullet> poolBullet = new Pool<Bullet>(factoryBullet, _startBulletCount);
            poolBullet.Init();
            
            _spawnerBullet = gameObject.AddComponent<SpawnerBullet>();
            _spawnerBullet.Init(poolBullet, _spawnBulletDelay, _maxBulletSpawnCount, _distanceRange);
        }
        
        private void InitPresenters()
        {
            // TODO: разделить класс игрока на презентер-вью и модель (убрать зависимоcть в _gameOverPresenter от плеера)
            _player.Init(_stats, _upgradeHandler, _gameLoopService);
            _spawnEnemyPresenter = new SpawnEnemyPresenter(_gameLoopService, _spawnerEnemy);
            _spawnBulletPresenter = new SpawnBulletPresenter(_gameLoopService, _spawnerBullet);
            _upgradePresenter.Init(_stats, _upgradeService, _gameLoopService, _gamePauseService);
            _gameOverPresenter = new GameOverPresenter(_gameLoopService, _player);
            _restartPresenter = new RestartGamePresenter(_gameLoopService, _gamePauseService, _restartView);
            _wavePresenter = new WavePresenter(_gameLoopService, _spawnerWave);
        }
        
        private void OnDestroy()
        {
            _gamePauseService.Dispose();
            _spawnerWave.Dispose();
            _spawnEnemyPresenter.Dispose();
            _spawnBulletPresenter.Dispose();
            _gameOverPresenter.Dispose();
            _restartPresenter.Dispose();
            _wavePresenter.Dispose();
        }
    }
}