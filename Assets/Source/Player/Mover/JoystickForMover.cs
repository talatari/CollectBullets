using UnityEngine;

namespace Source.Player.Movement
{
    public class JoystickForMover : JoystickHandler
    {
        [SerializeField] private PlayerMover _playerMover;
        
        private void Update()
        {
            if (_playerMover == null)
                return;
            
            if (_inputVector.x != 0 || _inputVector.y != 0)
            {
                _playerMover.Move(new Vector3(_inputVector.x, 0, _inputVector.y));
                _playerMover.Rotate(new Vector3(_inputVector.x, 0, _inputVector.y));
            }
            else
            {
                _playerMover.Move(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
                _playerMover.Rotate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            }
        }
    }
}