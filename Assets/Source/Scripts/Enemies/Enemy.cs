using System;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    [RequireComponent(typeof(Mover))]
    public class Enemy : MonoBehaviour
    {
        private Mover _mover;
        
        private void Awake()
        {
            _mover = GetComponent<Mover>();
        }

        public void SetTarget(Transform target)
        {
            if (_mover == null)
                return;
            
            _mover.SetTarget(target);
        }
    }
}