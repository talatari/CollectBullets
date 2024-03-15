using System;
using UnityEngine;

namespace Source.Codebase.Bullets
{
    public class BulletPointer : MonoBehaviour
    {
        private const int ScreenBorderCount = 4;
        
        [SerializeField] private Transform _pointer;
        
        private Transform _player;
        private Camera _camera;
        private float _radius;
        private float _speed = 20f;
        private float _ground = 0.75f;

        private void Start() => 
            _camera = Camera.main;

        private void LateUpdate()
        {
            if (_player == null)
                return;
            
            if (transform.position.y > _ground)
            {
                _pointer.gameObject.SetActive(false);
                
                return;
            }
            
            _pointer.gameObject.SetActive(true);
            
            Vector3 playerPosition = _player.position;
            Vector3 toPointer = transform.position - playerPosition;
            Ray ray = new Ray(playerPosition, toPointer);
            
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

            float rayMinDistance = Mathf.Infinity;

            for (int i = 0; i < ScreenBorderCount; i++)
                if (planes[i].Raycast(ray, out float distance))
                    if (distance < rayMinDistance)
                        rayMinDistance = distance;

            if (toPointer.magnitude < rayMinDistance)
            {
                _pointer.gameObject.SetActive(false);
                
                return;
            }
            
            _pointer.gameObject.SetActive(true);
            
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
            
            _pointer.position = Vector3.Lerp(
                _pointer.position, _camera.WorldToScreenPoint(worldPosition), _speed * Time.deltaTime);
        }

        private void SetRotation(Vector3 playerPosition)
        {
            float flip = -1f;
            Vector3 directionToEnemy = (transform.position - playerPosition).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
            _pointer.rotation = Quaternion.Euler(0f, 0f, flip * lookRotation.eulerAngles.y);
        }
    }
}





















