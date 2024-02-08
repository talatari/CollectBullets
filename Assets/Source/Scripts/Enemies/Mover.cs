using System;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        
        private Transform _target;

        private void Update()
        {
            if (_target == null)
                return;

            Vector3 position = transform.position;
            
            position = Vector3.MoveTowards(
                position, new Vector3(_target.position.x, position.y, _target.position.z), _speed * Time.deltaTime);
            
            transform.position = position;
        }
        
        public void SetTarget(Transform target)
        {
            if (target == null) 
                throw new ArgumentNullException(nameof(target));

            _target = target;
        }
    }
}