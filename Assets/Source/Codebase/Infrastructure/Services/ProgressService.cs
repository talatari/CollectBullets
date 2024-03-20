using System;
using System.Collections.Generic;
using System.Linq;
using Source.Codebase.Infrastructure.SaveLoadData;
using Source.Codebase.Upgrades;

namespace Source.Codebase.Infrastructure.Services
{
    public class ProgressService : IDisposable
    {
        private readonly SaveLoadService _saveLoadService;
        private readonly GameLoopMediator _gameLoopMediator;
        
        private List<UpgradeModel> _upgradeModels;
        private PlayerProgress _playerProgress;
        private int _waveNumberCompleted;
        private int _countKeySpawned;

        public ProgressService(
            SaveLoadService saveLoadService, GameLoopMediator gameLoopMediator, List<UpgradeModel> upgradeModels)
        {
            _saveLoadService = saveLoadService ?? throw new ArgumentNullException(nameof(saveLoadService));
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));
            _upgradeModels = upgradeModels ?? throw new ArgumentNullException(nameof(upgradeModels));

            _gameLoopMediator.WaveCompleted += OnSetCountWaveCompleted;
            _gameLoopMediator.KeySpawned += OnKeySpawned;
            _gameLoopMediator.KeyUsed += OnKeyUsed;
            _gameLoopMediator.GameOver += OnSaveResetProgress;
        }

        public void Dispose()
        {
            _gameLoopMediator.WaveCompleted -= OnSetCountWaveCompleted;
            _gameLoopMediator.KeySpawned -= OnKeySpawned;
            _gameLoopMediator.KeyUsed -= OnKeyUsed;
            _gameLoopMediator.GameOver -= OnSaveResetProgress;
        }

        public void Init() => 
            LoadPlayerProgress();

        public void SaveUpgrades(List<UpgradeModel> upgradeModels)
        {
            if (upgradeModels == null) 
                throw new ArgumentNullException(nameof(upgradeModels));
            
            _playerProgress.SetUpgradeProgresses(
                upgradeModels.Select(upgradeModel => UpgradeProgress.FromModel(upgradeModel)).ToList());
            
            _saveLoadService.SavePlayerProgress();
        }

        public bool TryGetUpgradeProgress(int upgradeModelId, out UpgradeProgress upgradeProgress)
        {
            upgradeProgress = _playerProgress.UpgradeProgresses.FirstOrDefault(
                progress => progress.Id == upgradeModelId);
            
            return upgradeProgress.Id != default;
        }

        private void LoadPlayerProgress()
        {
            _playerProgress = _saveLoadService.LoadPlayerProgress();

            foreach (UpgradeModel upgradeModel in _upgradeModels)
                if (TryGetUpgradeProgress(upgradeModel.Id, out UpgradeProgress upgradeProgress))
                    upgradeModel.UpgradeTo(upgradeProgress.CurrentLevel);
            
            _waveNumberCompleted = _playerProgress.CountWaveCompleted;
            _countKeySpawned = _playerProgress.CountKeySpawned;

            _gameLoopMediator.NotifyWaveNumberCompletedLoaded(_waveNumberCompleted);
            _gameLoopMediator.NotifyCountKeySpawnedLoaded(_countKeySpawned);
        }

        private void OnSetCountWaveCompleted(int waveNumberCompleted)
        {
            _waveNumberCompleted = waveNumberCompleted;
            _playerProgress.SetCountWaveCompleted(_waveNumberCompleted);
            _saveLoadService.SavePlayerProgress();
        }

        private void OnKeySpawned()
        {
            _countKeySpawned++;
            _playerProgress.SetCountKeySpawned(_countKeySpawned);
            _saveLoadService.SavePlayerProgress();
        }

        private void OnKeyUsed()
        {
            _countKeySpawned--;
            _playerProgress.SetCountKeySpawned(_countKeySpawned);
            _saveLoadService.SavePlayerProgress();
        }

        private void OnSaveResetProgress()
        {
            _playerProgress = _saveLoadService.CreateNewPlayerProgress();
            _countKeySpawned = 0;
            // _playerProgress.SetCountWaveCompleted(0);
            // _playerProgress.SetCountKeySpawned(0);
            // _saveLoadService.SavePlayerProgress();
        }
    }
}