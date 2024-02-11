using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Abilities
{
    public class AbilityButton : MonoBehaviour
    {
        [SerializeField] private GameObject _abilityView;
        [SerializeField] private Button _buttonClick;
    
        private void Start() => 
            _buttonClick.onClick.AddListener(OnClick);

        private void OnDestroy() => 
            _buttonClick.onClick.RemoveListener(OnClick);

        public void OnClick()
        {
            if (_abilityView == null)
                return;
            
            _abilityView.SetActive(false);
        }
    }
}