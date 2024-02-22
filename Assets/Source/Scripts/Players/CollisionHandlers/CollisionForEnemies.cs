using System;
using Source.Scripts.Enemies;
using UnityEngine;

namespace Source.Scripts.Players.CollisionHandlers
{
    public class CollisionForEnemies : CollisionHandler
    {
        private const float RatioIncrement = 0.15f;
        
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private RadiusEnemyDetectChanger _radiusEnemyDetectChanger;
        
        private Player _player;
        private Collider[] _enemyColliders = new Collider[MaxOverlap];
        private Vector3 _rotateDirection;
        private float _freeze;
        private int _baseFreeze;
        private bool _isInit;

        public void Init(Player player, int freeze)
        {
            if (player == null) 
                throw new ArgumentNullException(nameof(player));
            if (freeze < 0) 
                throw new ArgumentOutOfRangeException(nameof(freeze));

            _player = player;
            
            _baseFreeze = freeze;
            _freeze = _baseFreeze;

            _isInit = true;
        }

        private void Update()
        {
            if (_isInit == false)
                return;
            
            OverlapEnemies();
        }

        public void SetFreeze(int freeze)
        {
            if (freeze < 0) 
                throw new ArgumentOutOfRangeException(nameof(freeze));

            float newFreeze = _baseFreeze + (freeze - _baseFreeze) * RatioIncrement;
            
            _freeze = newFreeze;
        }

        private void OverlapEnemies()
        {
            int enemiesAmount = Physics.OverlapSphereNonAlloc(
                transform.position, _radiusEnemyDetectChanger.Radius, _enemyColliders, _enemyLayer);

            if (enemiesAmount == 0)
            {
                _player.RotateToEnemy(Vector3.zero);
                _player.StopShooting();
                
                return;
            }

            for (int i = 0; i < enemiesAmount; i++)
            {
                float distance = 0;
                float magnitude;
            
                if (_enemyColliders[i].TryGetComponent(out Enemy enemy))
                {
                    if (_freeze > _baseFreeze)
                        enemy.Freeze(_freeze);
                    
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
            
            _player.RotateToEnemy(_rotateDirection);
        }
    }
}