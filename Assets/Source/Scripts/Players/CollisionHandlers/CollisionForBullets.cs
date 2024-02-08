using System;
using Source.Scripts.Bullets;
using UnityEngine;

namespace Source.Scripts.Players.CollisionHandlers
{
    public class CollisionForBullets : CollisionHandler
    {
        [SerializeField] private LayerMask _bulletLayer;
        [SerializeField] private float _radiusPickUpBullets = 2f;
        
        private Player _player;
        private Collider[] _bulletColliders = new Collider[MaxOverlap];
        
        public event Action BulletCollected;
        
        public void Init(Player player)
        {
            if (player == null) 
                throw new ArgumentNullException(nameof(player));
            
            _player = player;
        }
        
        private void FixedUpdate() => 
            OverlapBullets();

        private void OverlapBullets()
        {
            if (_player.CollectedBullets >= _player.ClipCapacityBullets)
                return;
            
            int bulletsAmount = Physics.OverlapSphereNonAlloc(
                transform.position, _radiusPickUpBullets, _bulletColliders, _bulletLayer);

            for (int i = 0; i < bulletsAmount; i++)
                if (_bulletColliders[i].TryGetComponent(out Bullet bullet))
                {
                    bullet.OnReleaseToPool();
                    BulletCollected?.Invoke();
                }
        }
    }
}