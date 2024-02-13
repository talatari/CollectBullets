using System.Collections.Generic;
using Source.Scripts.SO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Source.Scripts.Upgrades
{
    public class UpgradePresenter : MonoBehaviour
    {
        [SerializeField] private UpgradeSriptableObject[] _upgrades;
        [SerializeField] private UpgradeView _upgradeLeftView;
        [SerializeField] private UpgradeView _upgradeMiddleView;
        [SerializeField] private UpgradeView _upgradeRightView;
        [SerializeField] private Button _rerollOnAdv;
        
        private Queue<UpgradeSriptableObject> _upgradesQueue = new ();
        private int _upgradeIndexLeft;
        private int _upgradeIndexMiddle;
        private int _upgradeIndexRight;
        private int _maxRange;

        private void OnEnable()
        {
            Time.timeScale = 0;
            
            OnSetUpgradesValue();

            if (_rerollOnAdv == null)
                return;
            
            _rerollOnAdv.onClick.AddListener(OnSetUpgradesValue);
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
            
            if (_rerollOnAdv == null)
                return;
            
            _rerollOnAdv.onClick.RemoveListener(OnSetUpgradesValue);
        }

        private void OnSetUpgradesValue()
        {
            LoadUpgrades();
            
            _upgradeLeftView.SetUpgrade(GetUpgades());
            _upgradeMiddleView.SetUpgrade(GetUpgades());
            _upgradeRightView.SetUpgrade(GetUpgades());
        }

        private void LoadUpgrades()
        {
            _maxRange = _upgrades.Length;
            _upgradeIndexMiddle = Random.Range(0, _maxRange);
            _upgradeIndexRight = Random.Range(0, _maxRange);
            
            _upgradesQueue.Enqueue(_upgrades[Random.Range(0, _maxRange)]);
            
            SetRandomUniqueIndex(_upgradeIndexMiddle);
            SetRandomUniqueIndex(_upgradeIndexRight);
        }

        private void SetRandomUniqueIndex(int index)
        {
            while (_upgradesQueue.Contains(_upgrades[index]))
                index = Random.Range(0, _maxRange);

            _upgradesQueue.Enqueue(_upgrades[index]);
        }

        private UpgradeSriptableObject GetUpgades() => 
            _upgradesQueue.Dequeue();
    }
}