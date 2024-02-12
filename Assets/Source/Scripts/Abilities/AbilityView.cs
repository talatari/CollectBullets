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
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _defaultValue;
        [SerializeField] private TMP_Text _separator;
        [SerializeField] private TMP_Text _upgradedValue;

        private const string CharPercent = "%";
        private const string Separator = "=>";
        private const string LevelText = "LEVEL";
        
        public void SetAbility(AbilitySriptableObject ability)
        {
            _background.sprite = ability.Icon;
            _name.text = ability.Name;

            if (ability.IsUpgradable)
            {
                _levelText.text = LevelText;
                _level.text = ability.Level.ToString();

                _defaultValue.text = ability.DefaultValue + CharPercent;
                _separator.text = Separator;
                _upgradedValue.text = ability.DefaultValue + ability.IncrementValue + CharPercent;
            }
            else
            {
                _levelText.text = "";
                _level.text = "";

                _defaultValue.text = "";
                _separator.text = "";
                _upgradedValue.text = "";
            }
        }

        public string GetNameAbility() => 
            _name.text;
    }
}