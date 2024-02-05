using UnityEngine;

namespace Source.Scripts.Enemies
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.5f;
        
        private Transform _target;

        private void Update()
        {
            if (_target == null)
                return;
            
            if (transform.position != _target.position)
                transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        }
        
        public void SetTarget(Transform target) => 
            _target = target;
    }
}