using System;
using Source.Scripts.Bullets;
using Source.Scripts.Enemies;
using UnityEngine;

namespace Source.Scripts.Players
{
    public class CollisionHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _bulletLayer;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _radiusPickUpBullets = 2f;
        [SerializeField] private RadiusChanger _radiusChanger;
        
        private const int MaxOverlap = 10;

        private Player _player;
        private Collider[] _bulletColliders = new Collider[MaxOverlap];
        private Collider[] _enemyColliders = new Collider[MaxOverlap];
        private Vector3 _rotateDirection;
        
        public event Action BulletCollected; 
        
        private void FixedUpdate()
        {
            OverlapBullets();

            OverlapEnemies();

            if (_rotateDirection != Vector3.zero)
                print($"_rotateDirection = {_rotateDirection}");
            
            _player.Rotate(_rotateDirection);
        }

        private void OverlapBullets()
        {
            int bulletsAmount = Physics.OverlapSphereNonAlloc(
                transform.position, _radiusPickUpBullets, _bulletColliders, _bulletLayer);

            for (int i = 0; i < bulletsAmount; i++)
                if (_player.CollectedBulletCount < _player.MaxCapacityBullets)
                    if (_bulletColliders[i].TryGetComponent(out Bullet bullet))
                    {
                        bullet.ReleaseToPool();
                        BulletCollected?.Invoke();
                    }
        }

        private void OverlapEnemies()
        {
            int enemiesAmount = Physics.OverlapSphereNonAlloc(
                transform.position, 
                _radiusChanger.Radius, 
                _enemyColliders, _enemyLayer);

            for (int i = 0; i < enemiesAmount; i++)
            {
                float distance = 0;
                
                if (_enemyColliders[i].TryGetComponent(out Enemy enemy))
                {
                    float magnitude = (_player.transform.position - enemy.transform.position).magnitude;
                    print($"magnitude = {magnitude}, enemy.name: {enemy.name}");
                    
                    if (distance < magnitude)
                    {
                        distance = magnitude;
                        _rotateDirection = enemy.transform.position;
                    }
                }
            }
        }

        public void Init(Player player)
        {
            if (player == null) 
                throw new ArgumentNullException(nameof(player));
            
            _player = player;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, _radiusChanger.Radius * 14);
        }
    }
}