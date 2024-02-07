using System;
using Source.Scripts.Enemies;
using UnityEngine;

namespace Source.Scripts.Players.CollisionHandlers
{
    public class CollisionForEnemies : CollisionHandler
    {
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private RadiusEnemyDetectChanger radiusEnemyDetectChanger;
        
        private Player _player;
        private Collider[] _enemyColliders = new Collider[MaxOverlap];
        private Vector3 _rotateDirection;
        
        public void Init(Player player)
        {
            if (player == null) 
                throw new ArgumentNullException(nameof(player));
            
            _player = player;
        }

        private void FixedUpdate() => 
            OverlapEnemies();

        private void OverlapEnemies()
        {
            int enemiesAmount = Physics.OverlapSphereNonAlloc(
                transform.position, radiusEnemyDetectChanger.Radius, _enemyColliders, _enemyLayer);

            if (enemiesAmount == 0)
                return;

            for (int i = 0; i < enemiesAmount; i++)
            {
                float distance = 0;
                float magnitude;
            
                if (_enemyColliders[i].TryGetComponent(out Enemy enemy))
                {
                    magnitude = (
                        _enemyColliders[i].ClosestPoint(transform.position) - _player.transform.position).magnitude;
                
                    if (distance < magnitude)
                    {
                        distance = magnitude;
                        _rotateDirection = enemy.transform.position - _player.transform.position;
                        _rotateDirection.y = 0;
                    }
                }
            }
            
            _player.Rotate(_rotateDirection);
        }
    }
}