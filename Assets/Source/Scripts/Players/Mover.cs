using UnityEngine;

namespace Source.Scripts.Players
{
    [RequireComponent(typeof(CharacterController))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 10.0f;
        [SerializeField] private float _rotateSpeed = 10.0f;
        [SerializeField] private float _gravityForce = 20.0f;

        private float _currentAttractionCharacter = 0;
        private CharacterController _characterController;

        private void Start() => 
            _characterController = GetComponent<CharacterController>();

        private void Update() => 
            GravityHandling();

        public void Move(Vector3 moveDirection)
        {
            moveDirection *= _moveSpeed;
            moveDirection.y = _currentAttractionCharacter;
            _characterController.Move(moveDirection * Time.deltaTime);
        }

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

        private void GravityHandling()
        {
            if (_characterController.isGrounded)
                _currentAttractionCharacter = 0;
            else
                _currentAttractionCharacter -= _gravityForce * Time.deltaTime;
        }
    }
}