using System;
using System.Collections.Generic;
using Source.Scripts.Infrastructure.SaveLoadData;
using Source.Scripts.Players.PlayerModels;
using Source.Scripts.SO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Source.Scripts.Upgrades
{
    public class UpgradePresenter : MonoBehaviour
    {
        [SerializeField] private UpgradeView _upgradeLeftView;
        [SerializeField] private UpgradeView _upgradeMiddleView;
        [SerializeField] private UpgradeView _upgradeRightView;
        [SerializeField] private Button _rerollOnAdv;
        
        private Queue<UpgradeSriptableObject> _upgradesQueue = new ();
        private int _upgradeIndexLeft;
        private int _upgradeIndexMiddle;
        private int _upgradeIndexRight;
        private int _maxRange;
        private Stats _stats;
        private UpgradeService _upgradeService;
        private PlayerProgress _playerProgress;
        private List<UpgradeModel> _upgradeModels;

        public void Init(Stats stats, UpgradeService upgradeService)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
            _upgradeService = upgradeService ?? throw new ArgumentNullException(nameof(upgradeService));
        }

        private void OnEnable()
        {
            Time.timeScale = 0;
            
            _upgradeLeftView.OnUpgradeButtonClick += OnUpgradeButtonClick;
            _upgradeMiddleView.OnUpgradeButtonClick += OnUpgradeButtonClick;
            _upgradeRightView.OnUpgradeButtonClick += OnUpgradeButtonClick;
            
            OnSetUpgradesValue();

            if (_rerollOnAdv == null)
                return;
            
            _rerollOnAdv.onClick.AddListener(OnSetUpgradesValue);
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
            
            _upgradeLeftView.OnUpgradeButtonClick -= OnUpgradeButtonClick;
            _upgradeMiddleView.OnUpgradeButtonClick -= OnUpgradeButtonClick;
            _upgradeRightView.OnUpgradeButtonClick -= OnUpgradeButtonClick;
            
            if (_rerollOnAdv == null)
                return;
            
            _rerollOnAdv.onClick.RemoveListener(OnSetUpgradesValue);
        }

        private void OnUpgradeButtonClick(int id) => 
            _upgradeService.Upgrade(id);

        private void OnSetUpgradesValue()
        {
            if (_upgradeService.TryGetUpgradeModels(out List<UpgradeModel> upgrades))
            {
                // TODO: сделать рандом
                _upgradeLeftView.SetUpgrade(upgrades[0]);
                _upgradeMiddleView.SetUpgrade(upgrades[1]);
                _upgradeRightView.SetUpgrade(upgrades[2]);
            }
        }

        // private UpgradeModel GetUpgades() => 
        //     _upgradesQueue.Dequeue();

        // private void LoadUpgrades()
        // {
        //     _maxRange = _upgrades.Length;
        //     _upgradeIndexMiddle = Random.Range(0, _maxRange);
        //     _upgradeIndexRight = Random.Range(0, _maxRange);
        //     
        //     _upgradesQueue.Enqueue(_upgrades[Random.Range(0, _maxRange)]);
        //     
        //     SetRandomUniqueIndex(_upgradeIndexMiddle);
        //     SetRandomUniqueIndex(_upgradeIndexRight);
        // }
        //
        // private void SetRandomUniqueIndex(int index)
        // {
        //     while (_upgradesQueue.Contains(_upgrades[index]))
        //         index = Random.Range(0, _maxRange);
        //
        //     _upgradesQueue.Enqueue(_upgrades[index]);
        // }
    }
}