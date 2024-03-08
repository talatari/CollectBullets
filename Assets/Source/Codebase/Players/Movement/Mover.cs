using System;
using UnityEngine;

namespace Source.Codebase.Players.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        
        private float _speed;

        [HideInInspector] public float CurrentAttractionCharacter;

        public void Init(float speed)
        {
            if (speed <= 0) 
                throw new ArgumentOutOfRangeException(nameof(speed));
            
            _speed = speed;
        }

        public void Move(Vector3 moveDirection)
        {
            moveDirection.Normalize();
            moveDirection *= _speed;
            moveDirection.y = CurrentAttractionCharacter;
            _characterController.Move(moveDirection * Time.deltaTime);
        }

        public void SetSpeed(int value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));

            _speed = value;
        }
    }
}