using Source.Scripts.Enemies;
using Source.Scripts.Infrastructure.Pools.Interfaces;
using Source.Scripts.Players.CollisionHandlers;
using Source.Scripts.SO;
using UnityEngine;

namespace Source.Scripts.Players.Projectiles
{
    public class ProjectileForPistol : CollisionHandler, IPoolable
    {
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private ProjectileScriptableObject _projectileScriptableObject;

        private int _damage;
        private float _speed;
        private Vector3 _direction;
        private Collider[] _enemyColliders = new Collider[MaxOverlap];
        private float _radius;

        private void Start()
        {
            _damage = _projectileScriptableObject.Damage;
            _speed = _projectileScriptableObject.Speed;

            float _diameter = transform.localScale.x;
            _radius = _diameter / 2;
            
            Destroy(gameObject, _projectileScriptableObject.LifeTime);
        }

        private void Update()
        {
            OverlapEnemies();
            
            Move();
        }

        public void SetDirection(Vector3 direction) => 
            _direction = direction;

        private void OverlapEnemies()
        {
            int enemiesAmount = Physics.OverlapSphereNonAlloc(
                transform.position, _radius, _enemyColliders, _enemyLayer);
            
            if (enemiesAmount == 0)
                return;
            
            // for (int i = 0; i < enemiesAmount; i++)
            if (_enemyColliders[0].TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }

        private void Move() => 
            transform.Translate(_direction * (_speed * Time.deltaTime));

        
        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            // TODO: реализовать пул проджектайлов, возможно эти методы нужны не в этом классе
        }

        public void Enable()
        {
            
        }

        public void Disable()
        {
            
        }

        public void OnReleaseToPool()
        {
            
        }
    }
}