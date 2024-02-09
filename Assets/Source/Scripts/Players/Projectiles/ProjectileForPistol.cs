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
            // TODO: QUESTION:
            // нормально так вычислять радиус? или как тут правильней обозначить магическое число 2?
            float _diameter = transform.localScale.x;
            _radius = _diameter / 2;
            // аналогичная история в классе RadiusEnemyDetectChanger
            
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
            
            // TODO: BAG:
            // когда несколько врагов долго преследуют игрока, они начинают сливаться воедино, так как
            // они не физические объекты и как следствие попадание в такую кучу врагов приводит к тому что
            // одна пуля дамажит всех разом. Нужно обрабатывать только первого из списка, либо разводить
            // врагов, чтобы они не сливались воедино.
            for (int i = 0; i < enemiesAmount; i++)
                if (_enemyColliders[i].TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(_damage);
                    Destroy(gameObject);
                    
                    break; // нормальная практика?
                }

            // такой хардкод думаю тоже не лучше, если мне нужно обработать коллизию с первым из списка объектом?
            // if (_enemyColliders[0].TryGetComponent(out Enemy enemy))
            // {
            //     enemy.TakeDamage(_damage);
            //     Destroy(gameObject);
            // }
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