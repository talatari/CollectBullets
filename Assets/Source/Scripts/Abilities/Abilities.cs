using System.Collections.Generic;
using Source.Scripts.SO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Abilities
{
    public class Abilities : MonoBehaviour
    {
        [SerializeField] private AbilitySriptableObject[] _abilities;
        [SerializeField] private AbilityView _abilityLeftView;
        [SerializeField] private AbilityView _abilityMiddleView;
        [SerializeField] private AbilityView _abilityRightView;
        
        private Queue<AbilitySriptableObject> _abilitiesQueue = new ();

        private void OnEnable() => 
            SetAbilityValue();

        private void SetAbilityValue()
        {
            LoadAbility();
            
            _abilityLeftView.SetAbility(GetAbility());
            _abilityMiddleView.SetAbility(GetAbility());
            _abilityRightView.SetAbility(GetAbility());
        }

        private void LoadAbility()
        {
            _abilitiesQueue.Enqueue(_abilities[Random.Range(0, _abilities.Length)]);
            _abilitiesQueue.Enqueue(_abilities[Random.Range(0, _abilities.Length)]);
            _abilitiesQueue.Enqueue(_abilities[Random.Range(0, _abilities.Length)]);
        }

        private AbilitySriptableObject GetAbility() => 
            _abilitiesQueue.Dequeue();
    }
}