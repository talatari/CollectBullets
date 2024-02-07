using UnityEngine;

namespace Source.Scripts.Players.Movement
{
    [RequireComponent(typeof(CharacterController), typeof(Mover))]
    public class Graviter : MonoBehaviour
    {
        [SerializeField] private float _gravityForce = 20.0f;
        
        private float _currentAttractionCharacter;
        private CharacterController _characterController;
        private Mover _mover;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _mover = GetComponent<Mover>();
        }

        private void Update() => 
            GravityHandling();

        private void GravityHandling()
        {
            if (_characterController.isGrounded)
                _currentAttractionCharacter = 0;
            else
                _currentAttractionCharacter -= _gravityForce * Time.deltaTime;

            _mover.CurrentAttractionCharacter = _currentAttractionCharacter;
        }
    }
}