using Source.Scripts.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Abilities
{
    public class AbilityView : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _defaultValue;
        [SerializeField] private TMP_Text _separator;
        [SerializeField] private TMP_Text _upgradedValue;

        private const string _charPercent = "%";
        
        public void SetAbility(AbilitySriptableObject ability)
        {
            _background.sprite = ability.Icon;
            _name.text = ability.Name;

            if (ability.IsUpgradable)
            {
                _level.text = ability.Level.ToString();
                _defaultValue.text = ability.DefaultValue + _charPercent;
                _upgradedValue.text = ability.DefaultValue + 1 + _charPercent; // TODO: переделать расчет значения
            }
            else
            {
                _level.text = "";
                _levelText.text = "";
                _defaultValue.text = "";
                _separator.text = "";
                _upgradedValue.text = "";
            }
        }
    }
}