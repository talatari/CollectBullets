using System;
using Source.Scripts.Players.PlayerStats;
using UnityEngine;

namespace Source.Scripts.Players.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        
        private float _speed;
        private CommonStats _commonStats;

        [HideInInspector] public float CurrentAttractionCharacter;

        public void Init(CommonStats commonStats)
        {
            _commonStats = commonStats ?? throw new ArgumentNullException(nameof(commonStats));
            _speed = _commonStats.Speed;
        }

        public void Move(Vector3 moveDirection)
        {
            moveDirection *= _speed;
            moveDirection.y = CurrentAttractionCharacter;
            _characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}