using Source.Codebase.Players.Movement;
using UnityEngine;

namespace Source.Codebase.Players.Joystick
{
    public class JoystickForMover : JoystickHandler
    {
        private static readonly int IsRun = Animator.StringToHash("IsRun");
        
        [SerializeField] private Mover _mover;
        [SerializeField] private Animator _animator;

        private Vector3 _previousPosition;
        private bool _isPreviousRun;
        
        private void Update()
        {
            if (_mover == null)
                return;

            if (_inputVector.x != 0 || _inputVector.y != 0)
                _mover.Move(new Vector3(_inputVector.x, 0, _inputVector.y));
            else
                _mover.Move(new Vector3(Input.GetAxisRaw(Horizontal), 0, Input.GetAxisRaw(Vertical)));
            
            float distance = Vector3.Distance(_previousPosition, _mover.transform.position);
            _previousPosition = _mover.transform.position;

            if (distance > 0.001f && _isPreviousRun == false)
                _animator.SetBool(IsRun, true);
            
            if (distance < 0.001f && _isPreviousRun)
                _animator.SetBool(IsRun, false);
            
            _isPreviousRun = distance > 0.001f;
        }
    }
}