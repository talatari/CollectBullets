using Source.Scripts.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Upgrades
{
    public class UpgradeView : MonoBehaviour
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
        
        public void SetUpgrade(UpgradeSriptableObject _upgrades)
        {
            _background.sprite = _upgrades.Icon;
            _name.text = _upgrades.Name;

            if (_upgrades.IsUpgradable)
            {
                _levelText.text = LevelText;
                _level.text = _upgrades.Level.ToString();

                _defaultValue.text = _upgrades.DefaultValue + CharPercent;
                _separator.text = Separator;
                _upgradedValue.text = _upgrades.DefaultValue + _upgrades.IncrementValue + CharPercent;
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