using UnityEngine;

namespace Source.Scripts.Players.Joystick
{
    public class JoystickForMover : JoystickHandler
    {
        [SerializeField] private Mover _mover;
        
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        
        private void Update()
        {
            if (_mover == null)
                return;
            
            if (_inputVector.x != 0 || _inputVector.y != 0)
            {
                _mover.Move(new Vector3(_inputVector.x, 0, _inputVector.y));
                _mover.Rotate(new Vector3(_inputVector.x, 0, _inputVector.y));
            }
            else
            {
                _mover.Move(new Vector3(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical)));
                _mover.Rotate(new Vector3(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical)));
            }
        }
    }
}