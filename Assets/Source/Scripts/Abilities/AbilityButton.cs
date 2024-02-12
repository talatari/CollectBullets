using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Abilities
{
    public class AbilityButton : MonoBehaviour
    {
        [SerializeField] private Canvas _abilityViewCanvas;
        [SerializeField] private Button _buttonClick;
        [SerializeField] private AbilityView _abilityView;
        
        public event Action<string> OnAbilityButtonClick; 
        
        private void Start() => 
            _buttonClick.onClick.AddListener(OnClick);

        private void OnDestroy() => 
            _buttonClick.onClick.RemoveListener(OnClick);

        public void OnClick()
        {
            if (_abilityViewCanvas == null)
                return;
            
            OnAbilityButtonClick?.Invoke(_abilityView.GetNameAbility());
            
            _abilityViewCanvas.gameObject.SetActive(false);
        }
    }
}