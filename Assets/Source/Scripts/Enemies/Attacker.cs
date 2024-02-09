using System;
using Source.Scripts.Players;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private LayerMask _playerLayer;
        
        private int _damage;
        private float _distanceAttack;
        private Collider[] _playerCollider = new Collider[1];

        public void Update() => 
            OverlapPlayer();

        private void OverlapPlayer()
        {
            int playerCollider = Physics.OverlapSphereNonAlloc(
                transform.position, _distanceAttack, _playerCollider, _playerLayer);
            
            if (playerCollider == 0)
                return;

            if (_playerCollider[0].TryGetComponent(out Player player))
                player.TakeDamage(_damage);
        }

        public void SetDamage(int damage)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            _damage = damage;
        }

        public void SetDistanceAttack(float distanceAttack)
        {
            if (distanceAttack <= 0) 
                throw new ArgumentOutOfRangeException(nameof(distanceAttack));
            
            _distanceAttack = distanceAttack;
        }
    }
}