using System;
using Source.Scripts.Players.PlayerStats;
using UnityEngine;

namespace Source.Scripts.Players.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class Mover : MonoBehaviour, IDisposable
    {
        [SerializeField] private CharacterController _characterController;
        
        private float _speed;
        private CommonStats _commonStats;

        [HideInInspector] public float CurrentAttractionCharacter;

        public void Init(CommonStats commonStats)
        {
            _commonStats = commonStats ?? throw new ArgumentNullException(nameof(commonStats));
            _commonStats.SpeedChanged += OnSetSpeed;
            _speed = _commonStats.Speed;
        }

        public void Dispose() => 
            _commonStats.SpeedChanged -= OnSetSpeed;

        public void Move(Vector3 moveDirection)
        {
            moveDirection *= _speed;
            moveDirection.y = CurrentAttractionCharacter;
            _characterController.Move(moveDirection * Time.deltaTime);
        }

        private void OnSetSpeed(float speed) => 
            _speed = speed;
    }
}