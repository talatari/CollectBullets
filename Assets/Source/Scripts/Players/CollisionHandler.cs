using System;
using Source.Scripts.Bullets;
using UnityEngine;

namespace Source.Scripts.Players
{
    public class CollisionHandler : MonoBehaviour
    {
        private Player _player;
        
        public event Action BulletCollected; 
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Bullet bullet))
            {
                if (_player.CollectedBulletCount < _player.MaxCapacityBullets)
                {
                    bullet.ReleaseToPool();
                    BulletCollected?.Invoke();
                }
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