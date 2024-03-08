using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Codebase.Upgrades
{
    public class UpgradeView : MonoBehaviour
    {
        private const string LevelText = "LEVEL";

        [SerializeField] private Image _defaultIcon;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _currentLevel;
        [SerializeField] private TMP_Text _currentValue;
        [SerializeField] private Button _buttonClick;

        private UpgradeModel _upgradeModel;
        
        public event Action<int> OnUpgradeButtonClick;
        
        private void Start() => 
            _buttonClick.onClick.AddListener(OnClick);

        private void OnDestroy() => 
            _buttonClick.onClick.RemoveListener(OnClick);

        public void SetUpgrade(UpgradeModel upgradeModel)
        {
            if (upgradeModel == null)
                return;
            
            _upgradeModel = upgradeModel;
            _icon.sprite = _upgradeModel.Config.Icon;
            _name.text = _upgradeModel.Name;
            _currentLevel.text = _upgradeModel.CurrentLevel.ToString();
            _currentValue.text = $"{_upgradeModel.CurrentValue} +" + _upgradeModel.IncrementValue;
            
            _levelText.text = LevelText;
        }

        private void OnClick()
        {
            if (_upgradeModel != null)
                OnUpgradeButtonClick?.Invoke(_upgradeModel.Id);
            
            ClearView();
        }

        private void ClearView()
        {
            _icon.sprite = _defaultIcon.sprite;
            _upgradeModel = null;
            _name.text = "";
            _currentLevel.text = "";
            _currentValue.text = "";
            _levelText.text = "";
        }
    }
}