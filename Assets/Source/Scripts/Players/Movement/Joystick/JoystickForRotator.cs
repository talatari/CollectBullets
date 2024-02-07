using UnityEngine;

namespace Source.Scripts.Players.Movement.Joystick
{
    public class JoystickForRotator : JoystickHandler
    {
        [SerializeField] private Rotator _rotator;

        public Vector3 EnemyPosition;
        
        private void Update()
        {
            if (_rotator == null)
                return;

            if (EnemyPosition == Vector3.zero)
                RotateToForward();
            else
                _rotator.Rotate(EnemyPosition);
            
        }

        private void RotateToForward()
        {
            if (_inputVector.x != 0 || _inputVector.y != 0)
                _rotator.Rotate(new Vector3(_inputVector.x, 0, _inputVector.y));
            else
                _rotator.Rotate(new Vector3(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical)));
        }
    }
}