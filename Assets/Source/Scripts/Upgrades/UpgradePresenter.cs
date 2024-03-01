using System;
using System.Collections.Generic;
using Source.Scripts.Infrastructure.SaveLoadData;
using Source.Scripts.Players.PlayerModels;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Source.Scripts.Upgrades
{
    public class UpgradePresenter : MonoBehaviour
    {
        [SerializeField] private Canvas _upgradesViewCanvas;
        [SerializeField] private UpgradeView _upgradeLeftView;
        [SerializeField] private UpgradeView _upgradeMiddleView;
        [SerializeField] private UpgradeView _upgradeRightView;
        [SerializeField] private Button _rerollOnAdv;
        
        private Queue<UpgradeModel> _upgradesQueue = new ();
        private int _upgradeIndexLeft;
        private int _upgradeIndexMiddle;
        private int _upgradeIndexRight;
        private int _maxRange;
        private Stats _stats;
        private UpgradeService _upgradeService;
        private PlayerProgress _playerProgress;
        private List<UpgradeModel> _upgradeModels;
        private bool _isInit;

        public void Init(Stats stats, UpgradeService upgradeService)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
            _upgradeService = upgradeService ?? throw new ArgumentNullException(nameof(upgradeService));

            _isInit = true;

            ShowViews();
        }

        private void ShowViews()
        {
            Time.timeScale = 0;
            
            _upgradeLeftView.OnUpgradeButtonClick += OnUpgradeButtonClick;
            _upgradeMiddleView.OnUpgradeButtonClick += OnUpgradeButtonClick;
            _upgradeRightView.OnUpgradeButtonClick += OnUpgradeButtonClick;
            
            if (_isInit)
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

        private void OnUpgradeButtonClick(int id)
        {
            _upgradeService.Upgrade(id);
            _upgradesViewCanvas.enabled = false;
            
            OnDisable();
        }

        private void OnSetUpgradesValue()
        {
            if (_upgradeService.TryGetUpgradeableModels(out List<UpgradeModel> upgradeModels))
            {
                if (upgradeModels.Count == 0)
                    return;
                
                _upgradeModels = upgradeModels;
                
                LoadUpgrades();
                
                _upgradeLeftView.SetUpgrade(GetUpgades());
                _upgradeMiddleView.SetUpgrade(GetUpgades());
                _upgradeRightView.SetUpgrade(GetUpgades());
            }
        }

        private UpgradeModel GetUpgades()
        {
            if (_upgradesQueue.Count == 0)
                return null;
            
            return _upgradesQueue.Dequeue();
        }

        private void LoadUpgrades()
        {
            _maxRange = _upgradeModels.Count;
            _upgradeIndexMiddle = Random.Range(0, _maxRange);
            _upgradeIndexRight = Random.Range(0, _maxRange);
            
            _upgradesQueue.Enqueue(_upgradeModels[Random.Range(0, _maxRange)]);
            
            SetRandomUniqueIndex(_upgradeIndexMiddle);
            SetRandomUniqueIndex(_upgradeIndexRight);
        }
        
        private void SetRandomUniqueIndex(int index)
        {
            if (_upgradeModels.Count == _upgradesQueue.Count)
                return;
            
            while (_upgradesQueue.Contains(_upgradeModels[index]))
                index = Random.Range(0, _maxRange);
        
            _upgradesQueue.Enqueue(_upgradeModels[index]);
        }
    }
}