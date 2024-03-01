using System;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    public class EnemyPointer : MonoBehaviour
    {
        [SerializeField] private Transform _enemyPointer;
        
        private Transform _player;
        private Camera _camera;
        private float _radius;
        private float _speed = 20f;

        private void Start() => 
            _camera = Camera.main;

        private void LateUpdate()
        {
            if (_player == null)
                return;
            
            Vector3 playerPosition = _player.position;
            Vector3 toEnemy = transform.position - playerPosition;
            Ray ray = new Ray(playerPosition, toEnemy);
            
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

            float rayMinDistance = Mathf.Infinity;

            for (int p = 0; p < 4; p++)
                if (planes[p].Raycast(ray, out float distance))
                    if (distance < rayMinDistance)
                        rayMinDistance = distance;

            if (toEnemy.magnitude < rayMinDistance)
            {
                _enemyPointer.gameObject.SetActive(false);
                
                return;
            }
            
            _enemyPointer.gameObject.SetActive(true);
            
            SetPosition(ray);
            SetRotation(playerPosition);
        }

        public void SetTarget(Transform player) => 
            _player = player ? player : throw new ArgumentNullException(nameof(player));

        public void SetRadius(float radius)
        {
            if (radius < 0) 
                throw new ArgumentOutOfRangeException(nameof(radius));

            _radius = radius;
        }

        private void SetPosition(Ray ray)
        {
            Vector3 worldPosition = ray.GetPoint(_radius);
            _enemyPointer.position = Vector3.Lerp(
                _enemyPointer.position, _camera.WorldToScreenPoint(worldPosition), _speed * Time.deltaTime);
        }

        private void SetRotation(Vector3 playerPosition)
        {
            float flip = -1f;
            Vector3 directionToEnemy = (transform.position - playerPosition).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
            _enemyPointer.rotation = Quaternion.Euler(0f, 0f, flip * lookRotation.eulerAngles.y);
        }
    }
}





















