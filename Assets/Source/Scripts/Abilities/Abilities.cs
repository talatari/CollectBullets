using System.Collections.Generic;
using Source.Scripts.SO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Source.Scripts.Abilities
{
    public class Abilities : MonoBehaviour
    {
        [SerializeField] private AbilitySriptableObject[] _abilities;
        [SerializeField] private AbilityView _abilityLeftView;
        [SerializeField] private AbilityView _abilityMiddleView;
        [SerializeField] private AbilityView _abilityRightView;
        [SerializeField] private Button _rerollOnAdv;
        
        private Queue<AbilitySriptableObject> _abilitiesQueue = new ();
        private int _abilityIndexLeft;
        private int _abilityIndexMiddle;
        private int _abilityIndexRight;
        private int _maxRange;

        private void OnEnable()
        {
            Time.timeScale = 0;
            
            OnSetAbilityValue();

            if (_rerollOnAdv == null)
                return;
            
            _rerollOnAdv.onClick.AddListener(OnSetAbilityValue);
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
            
            if (_rerollOnAdv == null)
                return;
            
            _rerollOnAdv.onClick.RemoveListener(OnSetAbilityValue);
        }

        private void OnSetAbilityValue()
        {
            LoadAbility();
            
            _abilityLeftView.SetAbility(GetAbility());
            _abilityMiddleView.SetAbility(GetAbility());
            _abilityRightView.SetAbility(GetAbility());
        }

        private void LoadAbility()
        {
            _maxRange = _abilities.Length;
            _abilityIndexMiddle = Random.Range(0, _maxRange);
            _abilityIndexRight = Random.Range(0, _maxRange);
            
            _abilitiesQueue.Enqueue(_abilities[Random.Range(0, _maxRange)]);
            
            SetRandomUniqueIndex(_abilityIndexMiddle);
            SetRandomUniqueIndex(_abilityIndexRight);
        }

        private void SetRandomUniqueIndex(int index)
        {
            while (_abilitiesQueue.Contains(_abilities[index]))
                index = Random.Range(0, _maxRange);

            _abilitiesQueue.Enqueue(_abilities[index]);
        }

        private AbilitySriptableObject GetAbility() => 
            _abilitiesQueue.Dequeue();
    }
}