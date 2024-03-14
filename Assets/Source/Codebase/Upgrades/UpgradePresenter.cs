using System;
using System.Collections.Generic;
using Source.Codebase.Infrastructure.SaveLoadData;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Players.PlayerModels;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Source.Codebase.Upgrades
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
        private UpgradeService _upgradeService;
        private PlayerProgress _playerProgress;
        private List<UpgradeModel> _upgradeModels;
        private GameLoopMediator _gameLoopMediator;
        private bool _isInit;
        private GamePauseService _gamePauseService;
        
        public void Init(
            UpgradeService upgradeService, GameLoopMediator gameLoopMediator, GamePauseService gamePauseService)
        {
            _upgradeService = upgradeService ?? throw new ArgumentNullException(nameof(upgradeService));
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));
            _gamePauseService = gamePauseService ?? throw new ArgumentNullException(nameof(gamePauseService));
            
            _gameLoopMediator.KeyUsed += OnShowUpgradeViews;

            _isInit = true;
        }
        
        private void OnDestroy()
        {
            if (_gameLoopMediator == null)
                return;
            
            _gameLoopMediator.KeyUsed -= OnShowUpgradeViews;
        }

        public void OnShowUpgradeViews()
        {
            _gamePauseService.InvokeByUI(true);
            _upgradesViewCanvas.enabled = true;
            
            _upgradeLeftView.OnUpgradeButtonClick += OnUpgradeButtonClick;
            _upgradeMiddleView.OnUpgradeButtonClick += OnUpgradeButtonClick;
            _upgradeRightView.OnUpgradeButtonClick += OnUpgradeButtonClick;
            
            if (_isInit)
                OnSetUpgradesValue();

            if (_rerollOnAdv == null)
                return;
            
            _rerollOnAdv.onClick.AddListener(OnSetUpgradesValue);
        }
        
        public void OnHideUpgradeViews()
        {
            _upgradeLeftView.OnUpgradeButtonClick -= OnUpgradeButtonClick;
            _upgradeMiddleView.OnUpgradeButtonClick -= OnUpgradeButtonClick;
            _upgradeRightView.OnUpgradeButtonClick -= OnUpgradeButtonClick;
            
            if (_rerollOnAdv == null)
                return;
            
            _rerollOnAdv.onClick.RemoveListener(OnSetUpgradesValue);

            _gamePauseService.InvokeByUI(false);
            _upgradesViewCanvas.enabled = false;
        }
        
        private void OnUpgradeButtonClick(int id)
        {
            _upgradeService.Upgrade(id);

            OnHideUpgradeViews();
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