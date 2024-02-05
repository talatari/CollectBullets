using System;
using Source.Scripts.Enemies;
using Source.Scripts.Players;
using Source.Scripts.Pool;
using Source.Scripts.Spawners;
using UnityEngine;

namespace Source.Scripts.Infrastructure
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private float _spawnDelay = 1f;

        private const string PathToEnemyPrefab = "Prefabs/Enemies/Enemy+";
        
        private Enemy _enemyPrefab;
        private CustomSpawner _enemySpawner;

        private void Start()
        {
            LoadPrefabs();

            EnemyFactory enemyFactory = new EnemyFactory(_enemyPrefab);
            EnemyPool enemyPool = new EnemyPool(enemyFactory);

            _enemySpawner = gameObject.AddComponent<CustomSpawner>();
            _enemySpawner.Construct(enemyPool, _spawnDelay);
        }

        private void LoadPrefabs()
        {
            _enemyPrefab = (Enemy)Resources.Load(PathToEnemyPrefab, typeof(Enemy));
            
            if (_enemyPrefab == null)
                throw new Exception("PathToEnemyPrefab not found.");
        }
    }
}