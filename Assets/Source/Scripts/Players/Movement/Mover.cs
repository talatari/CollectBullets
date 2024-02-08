using UnityEngine;

namespace Source.Scripts.Players.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 10.0f;

        public float CurrentAttractionCharacter;
        private CharacterController _characterController;

        private void Start() => 
            _characterController = GetComponent<CharacterController>();

        public void Move(Vector3 moveDirection)
        {
            moveDirection *= _moveSpeed;
            moveDirection.y = CurrentAttractionCharacter;
            _characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}