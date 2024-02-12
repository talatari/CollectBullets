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

        private void OnEnable()
        {
            _abilityButtonLeft.OnAbilityButtonClick += UpgradeAbility;
            _abilityButtonMiddle.OnAbilityButtonClick += UpgradeAbility;
            _abilityButtonRight.OnAbilityButtonClick += UpgradeAbility;
        }

        private void OnDisable()
        {
            _abilityButtonLeft.OnAbilityButtonClick -= UpgradeAbility;
            _abilityButtonMiddle.OnAbilityButtonClick -= UpgradeAbility;
            _abilityButtonRight.OnAbilityButtonClick -= UpgradeAbility;
        }

        private void UpgradeAbility(string abilityName) => 
            Player.UpgradeAbility(abilityName);
    }
}