using System;
using Source.Scripts.Bullets;
using Source.Scripts.Enemies;
using Source.Scripts.Infrastructure.Bullets;
using Source.Scripts.Infrastructure.Enemies;
using Source.Scripts.Players;
using Source.Scripts.Spawners;
using UnityEngine;

namespace Source.Scripts.Infrastructure
{
    public class CompositionRoot : MonoBehaviour
    {
        [Header("Enemies")]
        [SerializeField] private int _startEnemyCount = 5;
        [SerializeField] private int _maxEnemySpawnCount = 10;
        [SerializeField] private float _spawnEnemyDelay = 1f;
        [SerializeField] private Transform _enemiesParent;
        
        [Header("Bullets")]
        [SerializeField] private int _startBulletCount = 100;
        [SerializeField] private int _maxBulletSpawnCount = 1000;
        [SerializeField] private float _spawnBulletDelay = 1f;
        [SerializeField] private Transform _bulletsParent;
        
        [Header("Others")]
        [SerializeField] private float _distanceRange = 30f;

        private const string PathToEnemyPrefab = "Prefabs/Enemies/Enemy+";
        private const string PathToBulletPrefab = "Prefabs/Bullets/Bullet+";
        private const string PathToKeyPrefab = "Prefabs/Keys/Key+";
        
        private Player _player;
        private Enemy _enemyPrefab;
        private SpawnerEnemy _spawnerEnemy;
        private Bullet _bulletPrefab;
        private SpawnerBullet _spawnerBullet;
        private Key _keyPrefab;
        private SpawnerKey _spawnerKey;

        private void Start()
        {
            _player = FindObjectOfType<Player>();
            
            if (_player == null)
                throw new Exception("Player not found.");
            
            TargetService targetService = new TargetService();
            targetService.SetTarget(_player);
            
            LoadPrefabs();

            FactoryEnemy factoryEnemy = new FactoryEnemy(_enemyPrefab, _enemiesParent, _distanceRange, targetService);
            PoolEnemy poolEnemy = new PoolEnemy(factoryEnemy, _startEnemyCount);
            poolEnemy.Init();

            _spawnerEnemy = gameObject.AddComponent<SpawnerEnemy>();
            _spawnerEnemy.Construct(poolEnemy, _spawnEnemyDelay, _maxEnemySpawnCount);
            _spawnerEnemy.StartSpawn();
            
            FactoryBullet factoryBullet = new FactoryBullet(_bulletPrefab, _bulletsParent, _distanceRange);
            PoolBullet poolBullet = new PoolBullet(factoryBullet, _startBulletCount);
            poolBullet.Init();
            
            _spawnerBullet = gameObject.AddComponent<SpawnerBullet>();
            _spawnerBullet.Construct(poolBullet, _spawnBulletDelay, _maxBulletSpawnCount);
            _spawnerBullet.StartSpawn();
        }

        private void LoadPrefabs()
        {
            _enemyPrefab = (Enemy)Resources.Load(PathToEnemyPrefab, typeof(Enemy));
            if (_enemyPrefab == null)
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