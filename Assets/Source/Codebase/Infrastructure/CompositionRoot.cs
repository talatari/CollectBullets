using System;
using System.Collections.Generic;
using Agava.YandexGames;
using Source.Codebase.Bullets;
using Source.Codebase.Chests;
using Source.Codebase.Enemies;
using Source.Codebase.Enemies.Waves;
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

        [Header("Keys")]
        [SerializeField] private Key _keyPrefab;
        [SerializeField] private Transform _keysParent;
        [SerializeField] private int _keyCount;
        [SerializeField] private int _spawnInterval;

        [Header("Others")]
        [SerializeField] private float _distanceRange;
        [SerializeField] private RestartGameView _restartView;
        [SerializeField] private CompositeChestView _compositeChestView;

        private FactoryDefaultStats _factoryDefaultStats;
        private Stats _stats;
        private List<UpgradeModel> _upgradeModels;
        private UpgradeHandler _upgradeHandler;
        private GamePauseService _gamePauseService;
        private GameLoopMediator _gameLoopMediator;
        private SaveLoadService _saveLoadService;
        private ProgressService _progressService;
        private UpgradeService _upgradeService;
        private KeyService _keyService;
        private TargetProvider _targetProvider;
        private ChestPresenter _chestPresenter;
        private SpawnEnemyPresenter _spawnEnemyPresenter;
        private SpawnBulletPresenter _spawnBulletPresenter;
        private Pool<Enemy> _poolEnemy;
        private SpawnerEnemy _spawnerEnemy;
        private SpawnerWave _spawnerWave;
        private SpawnerBullet _spawnerBullet;
        private SpawnerKey _spawnerKey;
        private GameOverPresenter _gameOverPresenter;
        private RestartGamePresenter _restartPresenter;
        private WavePresenter _wavePresenter;

        private void Start()
        {
            CheckSerializedFields();

            CreateModels();
            
            CreateServices();

            InitServices();

            InitSpawners();

            InitPresenters();
            
            _progressService.Init();
            _upgradeService.Init();

            _gameLoopMediator.NotifyStartGame();
            
#if UNITY_WEBGL && !UNITY_EDITOR
            YandexGamesSdk.GameReady();
#endif
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
            
            if (_compositeChestView == null)
                throw new ArgumentNullException(nameof(_compositeChestView));
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
            _gamePauseService = new GamePauseService();
            _gameLoopMediator = new GameLoopMediator();
            _saveLoadService = new SaveLoadService(_upgradeModels);
            _progressService = new ProgressService(_saveLoadService, _gameLoopMediator, _upgradeModels);
            _upgradeService = new UpgradeService(_progressService, _upgradeModels);
            _keyService = new KeyService();
            _targetProvider = new TargetProvider();
            _chestPresenter = new ChestPresenter();
        }

        private void InitServices()
        {
            _gamePauseService.Init();
            _saveLoadService.Init();
            _targetProvider.Init(_player.transform);
            _compositeChestView.Init(_targetProvider);
            _chestPresenter.Init(_compositeChestView, _gameLoopMediator);
        }

        private void InitSpawners()
        {
            InitEnemySpawner();

            InitBulletSpawner();
            
            InitKeySpawner();
        }

        private void InitEnemySpawner()
        {
            FactoryEnemy factoryEnemy = new FactoryEnemy(_enemyPrefabs, _container, _targetProvider, _stats.CommonStats);
            _poolEnemy = new Pool<Enemy>(factoryEnemy, _waveConfig.DefaultCount);
            _poolEnemy.Init();
            
            _spawnerEnemy = gameObject.AddComponent<SpawnerEnemy>();
            _spawnerEnemy.Init(_poolEnemy, _maxEnemySpawnCount, _distanceRange);
            _spawnerWave = new SpawnerWave(_spawnerEnemy, _waveConfig);
        }

        private void InitBulletSpawner()
        {
            FactoryBullet factoryBullet = new FactoryBullet(_bulletPrefab, _bulletsParent, _targetProvider);
            Pool<Bullet> poolBullet = new Pool<Bullet>(factoryBullet, _startBulletCount);
            poolBullet.Init();
            
            _spawnerBullet = gameObject.AddComponent<SpawnerBullet>();
            _spawnerBullet.Init(poolBullet, _spawnBulletDelay, _maxBulletSpawnCount, _distanceRange);
        }

        private void InitKeySpawner()
        {
            FactoryKey factoryKey = new FactoryKey(_keyPrefab, _keysParent, _targetProvider);
            Pool<Key> poolKey = new Pool<Key>(factoryKey, _keyCount);
            poolKey.Init();
            
            _spawnerKey = gameObject.AddComponent<SpawnerKey>();
            _spawnerKey.Init(poolKey, _keyCount, _distanceRange);
            _keyService.Init(_spawnerKey, _gameLoopMediator, _spawnInterval);
        }

        private void InitPresenters()
        {
            _player.Init(_stats, _upgradeHandler, _gameLoopMediator);
            _spawnEnemyPresenter = new SpawnEnemyPresenter(_gameLoopMediator, _spawnerEnemy);
            _spawnBulletPresenter = new SpawnBulletPresenter(_gameLoopMediator, _spawnerBullet);
            _upgradePresenter.Init(_upgradeService, _gameLoopMediator, _gamePauseService);
            _gameOverPresenter = new GameOverPresenter(_gameLoopMediator, _player);
            _restartPresenter = new RestartGamePresenter(_gameLoopMediator, _gamePauseService, _restartView);
            _wavePresenter = new WavePresenter(_gameLoopMediator, _spawnerWave, _keyService);
        }
        
        private void OnDestroy()
        {
            _gamePauseService?.Dispose();
            _progressService?.Dispose();
            _chestPresenter?.Dispose();
            _spawnerWave?.Dispose();
            _spawnEnemyPresenter?.Dispose();
            _spawnBulletPresenter?.Dispose();
            _gameOverPresenter?.Dispose();
            _restartPresenter?.Dispose();
            _wavePresenter?.Dispose();
            _keyService?.Dispose();
        }
    }
}