using Source.Enemies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace Source.Spawner
{
    public class SpawnerEnemy : MonoBehaviour
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private int _enemyCount = 10;
        [SerializeField] private GameObject _enemyContainer;
        [SerializeField] private float _minRange = -10f;
        [SerializeField] private float _maxRange = 10f;
        
        // private Pool<Enemy> _enemyPool;
        private IObjectPool<Enemy> _pool;

        private void Awake() => 
            _pool = new ObjectPool<Enemy>(createFunc: Init, actionOnGet: Get, actionOnRelease: Release);

        private void Start()
        {
            for (int i = 0; i < _enemyCount; i++)
                _pool.Get();
        }

        private Enemy Init()
        {
            var enemy = Instantiate(_enemyPrefab, _enemyContainer.transform, true);
            enemy.AddComponent<ReturnToPool>().Init(_pool, enemy);
            enemy.name = $"Enemy{enemy.GetInstanceID()}";
            
            enemy.transform.position = new Vector3(
                Random.Range(_minRange, _maxRange), _enemyPrefab.transform.position.y, 
                Random.Range(_minRange, _maxRange));

            Release(enemy);
            
            return enemy;
        }

        private void Get(Enemy enemy) => enemy.gameObject.SetActive(true);
        private void Release(Enemy enemy) => enemy.gameObject.SetActive(false);
    }
}