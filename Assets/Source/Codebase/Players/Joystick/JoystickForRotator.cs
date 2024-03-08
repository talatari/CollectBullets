using Source.Codebase.Players.Movement;
using UnityEngine;

namespace Source.Codebase.Players.Joystick
{
    public class JoystickForRotator : JoystickHandler
    {
        [SerializeField] private Rotator _rotator;

        private Vector3 _enemyPosition;
        
        private void Update()
        {
            if (_rotator == null)
                return;

            if (_enemyPosition == Vector3.zero)
                RotateToForward();
            else
                _rotator.Rotate(_enemyPosition);
        }

        public void SetEnemyPosition(Vector3 enemyPosition) => 
            _enemyPosition = enemyPosition;

        private void RotateToForward()
        {
            if (_inputVector.x != 0 || _inputVector.y != 0)
                _rotator.Rotate(new Vector3(_inputVector.x, 0, _inputVector.y));
            else
                _rotator.Rotate(new Vector3(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical)));
        }
    }
}