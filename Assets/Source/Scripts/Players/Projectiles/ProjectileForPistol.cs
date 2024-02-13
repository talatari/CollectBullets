using System;
using System.Linq;
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
        private int _burning;
        private float _speed;
        private Vector3 _direction;
        private Collider[] _enemyColliders = new Collider[MaxOverlap];
        private float _radius;

        public void Init(int damage, int burning)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            if (burning < 0)
                throw new ArgumentOutOfRangeException(nameof(burning));
            
            _damage = damage;
            _burning = burning;
        }

        public void SetDamage(int damage)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            _damage = damage;
        }

        public void SetBurning(int burning)
        {
            if (burning < 0)
                throw new ArgumentOutOfRangeException(nameof(burning));

            _burning = burning;
        }
        
        private void Start()
        {
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
            
            // TODO: проверить остальные Overlap методы на соответствие одному виду использования
            if (_enemyColliders.First(x => x != null).TryGetComponent(out Enemy enemy))
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