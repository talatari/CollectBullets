using System;
using System.Collections.Generic;
using Source.Scripts.Enemies;
using Source.Scripts.Infrastructure.Factories;
using Source.Scripts.Infrastructure.Pools;
using Source.Scripts.Infrastructure.Spawners;
using Source.Scripts.Keys;
using Source.Scripts.Players;
using Source.Scripts.Players.PlayerStats;
using Source.Scripts.SO;
using UnityEngine;

namespace Source.Scripts.Infrastructure
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private PlayerScriptableObject _playerScriptableObject;
        
        [Header("Enemies")]
        [SerializeField] private int _startEnemyCount = 20;
        [SerializeField] private int _maxEnemySpawnCount = 100;
        [SerializeField] private float _spawnEnemyDelay = 1f;
        [SerializeField] private Transform _enemiesParent;
        
        [Header("Bullets")]
        [SerializeField] private int _startBulletCount = 20;
        [SerializeField] private int _maxBulletSpawnCount = 100;
        [SerializeField] private float _spawnBulletDelay = 1f;
        [SerializeField] private Transform _bulletsParent;
        
        [Header("Others")]
        [SerializeField] private float _distanceRange = 30f;
        
        private const string PathToEnemyMeleePrefab = "Prefabs/Enemies/Melee+";
        private const string PathToEnemyRangePrefab = "Prefabs/Enemies/Range+";
        private const string PathToBulletPrefab = "Prefabs/Bullets/Bullet+";
        private const string PathToKeyPrefab = "Prefabs/Keys/Key+";
        
        private Stats _stats;
        private List<Enemy> _enemyPrefabs = new ();
        private SpawnerEnemy _spawnerEnemy;
        private Bullet _bulletPrefab;
        private SpawnerBullet _spawnerBullet;
        private Key _keyPrefab;
        private SpawnerKey _spawnerKey;
        
        private void Start()
        {
            if (_player == null)
                throw new ArgumentNullException(nameof(_player));

            _stats = new Stats(
                new DamageStats(
                    _playerScriptableObject.Damage, 
                    _playerScriptableObject.ClipCapacity,
                    _playerScriptableObject.Burning,
                    _playerScriptableObject.ShootingDelay), 
                new HealthStats(
                    _playerScriptableObject.MaxHealth,
                    _playerScriptableObject.Regeneration,
                    _playerScriptableObject.Vampirism),
                new CommonStats(
                    _playerScriptableObject.Magnet));
            
            _player.Init(_stats);
            
            TargetProvider targetProvider = new TargetProvider();
            targetProvider.SetTarget(_player.transform);
            
            LoadPrefabs();
            
            FactoryEnemy factoryEnemy = new FactoryEnemy(_enemyPrefabs, _enemiesParent, targetProvider);
            Pool<Enemy> poolEnemy = new Pool<Enemy>(factoryEnemy, _startEnemyCount);
            poolEnemy.Init();

            _spawnerEnemy = gameObject.AddComponent<SpawnerEnemy>();
            _spawnerEnemy.Construct(poolEnemy, _spawnEnemyDelay, _maxEnemySpawnCount, _distanceRange);
            _spawnerEnemy.StartSpawn();
            
            FactoryBullet factoryBullet = new FactoryBullet(_bulletPrefab, _bulletsParent);
            Pool<Bullet> poolBullet = new Pool<Bullet>(factoryBullet, _startBulletCount);
            poolBullet.Init();
            
            _spawnerBullet = gameObject.AddComponent<SpawnerBullet>();
            _spawnerBullet.Construct(poolBullet, _spawnBulletDelay, _maxBulletSpawnCount, _distanceRange);
            _spawnerBullet.StartSpawn();
        }

        private void LoadPrefabs()
        {
            _enemyPrefabs.Add((Enemy)Resources.Load(PathToEnemyMeleePrefab, typeof(Enemy)));
            _enemyPrefabs.Add((Enemy)Resources.Load(PathToEnemyRangePrefab, typeof(Enemy)));
            // TODO: переделать на загрузку префабов через SO
            if (_enemyPrefabs == null)
                throw new Exception("PathToEnemyPrefab not found.");
            
            _bulletPrefab = (Bullet)Resources.Load(PathToBulletPrefab, typeof(Bullet));
            if (_bulletPrefab == null)
                throw new Exception("PathToBulletPrefab not found.");
            
            _keyPrefab = (Key)Resources.Load(PathToKeyPrefab, typeof(Key));
            if (_keyPrefab == null)
                throw new Exception("PathToKeyPrefab not found.");
        }
    }
}