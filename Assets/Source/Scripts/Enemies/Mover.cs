using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Source.Scripts.Enemies
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        
        private float _speed;
        private float _distanceAttack;
        private float _freeze;
        private Transform _target;

        public void Init(float speed, float distanceAttack)
        {
            if (speed <= 0) 
                throw new ArgumentOutOfRangeException(nameof(speed));
            if (distanceAttack < 0) 
                throw new ArgumentOutOfRangeException(nameof(distanceAttack));

            _speed = speed;
            _distanceAttack = distanceAttack;
        }

        private void Update()
        {
            if (_target == null)
                return;
            
            if (_agent == null)
                return;
            
            if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
            {
                _agent.SetDestination(_target.position);
                _agent.speed = _speed - _freeze;
                _agent.stoppingDistance = _distanceAttack;
            }
            
            _agent.transform.rotation = Quaternion.identity;
        }

        public void SetTarget(Transform target) => 
            _target = target;

        public void Freeze(float freeze)
        {
            if (freeze < 0)
                throw new ArgumentOutOfRangeException(nameof(freeze));
            
            StartCoroutine(Freez(freeze));
        }
        
        private IEnumerator Freez(float freeze)
        {
            _freeze = freeze;
            
            yield return new WaitForSeconds(1f);

            _freeze = 0;
        }
    }
}