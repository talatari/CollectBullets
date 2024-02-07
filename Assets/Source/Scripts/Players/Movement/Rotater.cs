using UnityEngine;

namespace Source.Scripts.Players.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class Rotater : MonoBehaviour
    {
        [SerializeField] private float _rotateSpeed = 10.0f;
        
        private CharacterController _characterController;

        private void Start() => 
            _characterController = GetComponent<CharacterController>();

        public void Rotate(Vector3 moveDirection)
        {
            if (_characterController.isGrounded == false)
                return;
            
            if (Vector3.Angle(transform.forward, moveDirection) > 0)
            {
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, _rotateSpeed, 0);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }
}