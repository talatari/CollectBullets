using System;
using Source.Scripts.Players.CollisionHandlers;
using Source.Scripts.Players.Projectiles;
using UnityEngine;

namespace Source.Scripts.Players.Weapons
{
    public class ProjectileForPistol : CollisionHandler
    {
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private ProjectileScriptableObject _projectileScriptableObject;

        private int _damage;
        private float _speed;
        private Vector3 _direction;
        private Collider[] _enemyColliders = new Collider[MaxOverlap];

        private void Start()
        {
            _damage = _projectileScriptableObject.Damage;
            _speed = _projectileScriptableObject.Speed;
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
                transform.position, transform.localScale.x / 2, _enemyColliders, _enemyLayer);
        }

        private void Move() => 
            transform.Translate(_direction * (_speed * Time.deltaTime));
    }
}