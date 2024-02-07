using UnityEngine;

namespace Source.Scripts.Players.Movement.Joystick
{
    public class JoystickForRotater : JoystickHandler
    {
        [SerializeField] private Rotater _rotater;

        private void Update()
        {
            if (_rotater == null)
                return;

            if (_inputVector.x != 0 || _inputVector.y != 0)
                _rotater.Rotate(new Vector3(_inputVector.x, 0, _inputVector.y));
            else
                _rotater.Rotate(new Vector3(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical)));
        }
    }
}