using System;
using System.Linq;
using Source.Codebase.Enemies;
using Source.Codebase.Infrastructure.Pools.Interfaces;
using Source.Codebase.Players.CollisionHandlers;
using Source.Codebase.SO;
using UnityEngine;

namespace Source.Codebase.Players.Projectiles
{
    public class ProjectileForPistol : CollisionHandler, IPoolable
    {
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private ProjectileScriptableObject _projectileScriptableObject;

        private int _damage;
        private int _burning;
        private int _burningDuration;
        private int _vampirism;
        private int _speed;
        private float _radius;
        private Vector3 _direction;
        private Collider[] _enemyColliders = new Collider[MaxOverlap];

        public event Action<ProjectileForPistol, int> Vampired; 

        public void Init(int damage, int burning, int burningDuration, int vampirism)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            if (burning < 0)
                throw new ArgumentOutOfRangeException(nameof(burning));
            if (burningDuration < 0)
                throw new ArgumentOutOfRangeException(nameof(burningDuration));
            if (vampirism < 0) 
                throw new ArgumentOutOfRangeException(nameof(vampirism));

            _damage = damage;
            _burning = burning;
            _burningDuration = burningDuration;
            _vampirism = vampirism;
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

        public void SetDamage(int value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _damage = value;
        }

        public void SetBurning(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _burning = value;
        }

        public void SetVampirism(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _vampirism = value;
        }

        public void SetDirection(Vector3 direction) => 
            _direction = direction;

        private void OverlapEnemies()
        {
            int enemiesAmount = Physics.OverlapSphereNonAlloc(
                transform.position, _radius, _enemyColliders, _enemyLayer);
            
            if (enemiesAmount == 0)
                return;
            
            if (_enemyColliders.First(enemyCollider => enemyCollider != null).TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(_damage);
                
                if (_burning > 0)
                    enemy.Burn(_burning, _burningDuration);

                if (_vampirism > 0)
                    Vampired?.Invoke(this, _vampirism);
                
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