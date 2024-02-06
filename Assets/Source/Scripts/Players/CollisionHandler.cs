using System;
using Source.Scripts.Bullets;
using UnityEngine;

namespace Source.Scripts.Players
{
    public class CollisionHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _bulletsLayer;
        [SerializeField] private float _radius = 2f;
        
        private Player _player;
        private static int _maxOverlap = 10;
        private Collider[] _results = new Collider[_maxOverlap];
        
        public event Action BulletCollected; 
        
        private void FixedUpdate()
        {
            int size = Physics.OverlapSphereNonAlloc(transform.position, _radius, _results, _bulletsLayer);

            for (int i = 0; i < size; i++)
                if (_player.CollectedBulletCount < _player.MaxCapacityBullets)
                    if (_results[i].TryGetComponent(out Bullet bullet))
                    {
                        bullet.ReleaseToPool();
                        BulletCollected?.Invoke();
                    }
        }
        
        public void Init(Player player)
        {
            if (player == null) 
                throw new ArgumentNullException(nameof(player));
            
            _player = player;
        }
    }
}