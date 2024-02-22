using System;
using System.Collections;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    public class Mover : MonoBehaviour
    {
        private float _speed;
        private float _freeze;
        private Transform _target;

        private void Update()
        {
            if (_target == null)
                return;
            
            if (_speed == 0)
                return;

            Vector3 position = transform.position;
            
            position = Vector3.MoveTowards(
                position, 
                new Vector3(_target.position.x, position.y, _target.position.z), 
                (_speed - _freeze) * Time.deltaTime);
            
            transform.position = position;
        }

        public void SetTarget(Transform target) => 
            _target = target;

        public void SetSpeed(float speed)
        {
            if (speed <= 0) 
                throw new ArgumentOutOfRangeException(nameof(speed));
            
            _speed = speed;
        }

        public void Freeze(float freeze)
        {
            if (freeze < 0)
                throw new ArgumentOutOfRangeException(nameof(freeze));
            
            StartCoroutine(Freez(freeze));
        }
        
        private IEnumerator Freez(float freeze)
        {
            _freeze = freeze;
            
            yield return new WaitForSeconds(1);

            _freeze = 0;
        }
    }
}