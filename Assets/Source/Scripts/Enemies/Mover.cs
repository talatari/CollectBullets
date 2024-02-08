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
            
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        }
        
        public void SetTarget(Transform target)
        {
            if (target == null) 
                throw new ArgumentNullException(nameof(target));

            _target = target;
        }
    }
}