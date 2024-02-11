using Source.Scripts.Players;
using Source.Scripts.Players.CollisionHandlers;
using Source.Scripts.SO;
using UnityEngine;

namespace Source.Scripts.Enemies
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
            int playerColliders = Physics.OverlapSphereNonAlloc(
                transform.position, _radius, _playerColliders, _playerLayer);
            
            if (playerColliders == 0)
                return;
            
            // for (int i = 0; i < playerColliders; i++)
            if (_playerColliders[0].TryGetComponent(out Player player))
            {
                player.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }

        private void Move() => 
            transform.Translate(_direction * (_speed * Time.deltaTime));
    }
}