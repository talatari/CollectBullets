using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Upgrades
{
    public class UpgradeView : MonoBehaviour
    {
        private const string CharPercent = "%";
        private const string LevelText = "LEVEL";
        private const string Separator = "=>";
        
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _currentLevel;
        [SerializeField] private TMP_Text _value;
        [SerializeField] private TMP_Text _separator;
        [SerializeField] private TMP_Text _upgradedValue;
        [SerializeField] private Canvas _abilityViewCanvas;
        [SerializeField] private Button _buttonClick;

        private int _id;
        
        public event Action<int> OnUpgradeButtonClick;
        
        private void Start() => 
            _buttonClick.onClick.AddListener(OnClick);

        private void OnDestroy() => 
            _buttonClick.onClick.RemoveListener(OnClick);

        public void SetUpgrade(UpgradeModel upgradeModel)
        {
            _background.sprite = upgradeModel.Config.Icon;
            _id = upgradeModel.Id;
            _name.text = upgradeModel.Name;
            _value.text = upgradeModel.CurrentValue.ToString();
            _currentLevel.text = upgradeModel.CurrentLevel.ToString();
            _upgradedValue.text = upgradeModel.GetNextValue().ToString();
            
            _separator.text = Separator;
            _levelText.text = LevelText;
        }

        private void OnClick()
        {
            OnUpgradeButtonClick?.Invoke(_id);
            
            _abilityViewCanvas.gameObject.SetActive(false);
        }
    }
}