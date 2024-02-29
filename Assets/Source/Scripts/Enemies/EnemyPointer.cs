using System;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    public class EnemyPointer : MonoBehaviour
    {
        private Transform _player;
        private Camera _camera;

        private void Start() => 
            _camera = Camera.main;

        private void LateUpdate()
        {
            if (_player == null)
                return;
            
            Vector3 playerPosition = _player.position;
            Vector3 fromPlayerToEnemy = transform.position - playerPosition;
            Ray ray = new Ray(playerPosition, fromPlayerToEnemy);
            Debug.DrawRay(playerPosition, fromPlayerToEnemy);

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        }

        public void SetTarget(Transform target) => 
            _player = target ? target : throw new ArgumentNullException(nameof(target));
    }
}