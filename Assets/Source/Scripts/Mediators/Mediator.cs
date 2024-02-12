using Sirenix.OdinInspector;
using Source.Scripts.Abilities;
using Source.Scripts.Players;
using UnityEngine;

namespace Source.Scripts.Mediators
{
    public class Mediator : MonoBehaviour
    {
        [Required, SceneObjectsOnly] public Player Player;
        [SerializeField] private AbilityButton _abilityButtonLeft;
        [SerializeField] private AbilityButton _abilityButtonMiddle;
        [SerializeField] private AbilityButton _abilityButtonRight;
    }
}