using UnityEngine;

namespace Source.Scripts.Players.Movement.Joystick
{
    public class JoystickForRotator : JoystickHandler
    {
        [SerializeField] private Rotator _rotator;

        private void Update()
        {
            if (_rotator == null)
                return;

            if (_inputVector.x != 0 || _inputVector.y != 0)
                _rotator.Rotate(new Vector3(_inputVector.x, 0, _inputVector.y));
            else
                _rotator.Rotate(new Vector3(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical)));
        }
    }
}