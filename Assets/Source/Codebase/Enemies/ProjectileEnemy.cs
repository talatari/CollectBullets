using System.Linq;
using Source.Codebase.Players;
using Source.Codebase.Players.CollisionHandlers;
using Source.Codebase.SO;
using UnityEngine;

namespace Source.Codebase.Enemies
{
    public class ProjectileEnemy : CollisionHandler
    {
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private ProjectileScriptableObject _projectileScriptableObject;

        private int _damage;
        private float _speed;
        private Vector3 _direction;
        private Collider[] _playerColliders = new Collider[MaxOverlap];
        private float _radius;
        private GameObject _gameObject;

        private void Start()
        {
            _damage = _projectileScriptableObject.Damage;
            _speed = _projectileScriptableObject.Speed;
            
            float _diameter = transform.localScale.x;
            _radius = _diameter / 2;

            _gameObject = gameObject;
            Destroy(_gameObject, _projectileScriptableObject.LifeTime);
        }

        private void Update()
        {
            OverlapEnemies();
            
            Move();
        }
        
        public void Destroy()
        {
            if (_gameObject != null)
                Destroy(gameObject);
        }

        public void SetDirection(Vector3 direction) => 
            _direction = direction;

        private void OverlapEnemies()
        {
            int playerColliders = Physics.OverlapSphereNonAlloc(
                transform.position, _radius, _playerColliders, _playerLayer);
            
            if (playerColliders == 0)
                return;
            
            if (_playerColliders.First(playerCollider => playerCollider != null).TryGetComponent(out Player player))
            {
                player.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }

        private void Move() => 
            transform.Translate(_direction * (_speed * Time.deltaTime));
    }
}