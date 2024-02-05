using System;
using Source.Scripts.Bullets;
using Source.Scripts.Enemies;
using Source.Scripts.Infrastructure.Bullets;
using Source.Scripts.Infrastructure.Enemies;
using Source.Scripts.Spawners;
using UnityEngine;

namespace Source.Scripts.Infrastructure
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private int _startEnemyCount = 5;
        [SerializeField] private int _maxEnemySpawnCount = 10;
        [SerializeField] private float _spawnDelay = 1f;
        [SerializeField] private float _distanceRange = 30f;
        [SerializeField] private int _startBulletCount = 5;
        [SerializeField] private int _maxBulletSpawnCount = 10;

        private const string PathToEnemyPrefab = "Prefabs/Enemies/Enemy+";
        private const string PathToBulletPrefab = "Prefabs/Bullets/Bullet+";
        private const string PathToKeyPrefab = "Prefabs/Keys/Key+";
        
        private Enemy _enemyPrefab;
        private SpawnerEnemy _spawnerEnemy;
        private Bullet _bulletPrefab;
        private SpawnerBullet _spawnerBullet;
        private Key _keyPrefab;
        private SpawnerKey _spawnerKey;

        private void Start()
        {
            LoadPrefabs();

            FactoryEnemy factoryEnemy = new FactoryEnemy(_enemyPrefab);
            PoolEnemy poolEnemy = new PoolEnemy(factoryEnemy, _startEnemyCount);
            poolEnemy.Init();

            _spawnerEnemy = gameObject.AddComponent<SpawnerEnemy>();
            _spawnerEnemy.Construct(poolEnemy, _spawnDelay, _distanceRange, _maxEnemySpawnCount);
            _spawnerEnemy.StartSpawn();
            
            FactoryBullet factoryBullet = new FactoryBullet(_bulletPrefab);
            PoolBullet poolBullet = new PoolBullet(factoryBullet, _startBulletCount);
            poolBullet.Init();
            
            _spawnerBullet = gameObject.AddComponent<SpawnerBullet>();
            _spawnerBullet.Construct(poolBullet, _spawnDelay, _distanceRange, _maxBulletSpawnCount);
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