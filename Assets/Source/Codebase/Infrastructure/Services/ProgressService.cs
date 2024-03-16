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
        private PlayerProgress _playerProgress;
        private int _waveNumberCompleted;
        private int _countKeySpawned;

        public ProgressService(SaveLoadService saveLoadService, GameLoopMediator gameLoopMediator)
        {
            _saveLoadService = saveLoadService ?? throw new ArgumentNullException(nameof(saveLoadService));
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));
            
            _gameLoopMediator.WaveCompleted += OnSetCountWaveCompleted;
            _gameLoopMediator.KeySpawned += OnKeySpawned;
            _gameLoopMediator.KeyUsed += OnKeyUsed;
        }

        public void Dispose()
        {
            _gameLoopMediator.WaveCompleted -= OnSetCountWaveCompleted;
            _gameLoopMediator.KeySpawned -= OnKeySpawned;
            _gameLoopMediator.KeyUsed -= OnKeyUsed;
        }

        public void Init() => 
            _playerProgress = _saveLoadService.LoadPlayerProgress();

        public void SaveUpgradesPlayerProgress(List<UpgradeModel> upgradeModels)
        {
            if (upgradeModels == null) 
                throw new ArgumentNullException(nameof(upgradeModels));

            _playerProgress.SetUpgradeProgresses(
                upgradeModels.Select(model => UpgradeProgress.FromModel(model)).ToList());
            _playerProgress.SetCountWaveCompleted(_waveNumberCompleted);
            _playerProgress.SetCountKeySpawned(_countKeySpawned);
            
            _saveLoadService.SavePlayerProgress();
        }

        public bool TryGetUpgradeProgress(int upgradeModelId, out UpgradeProgress upgradeProgress)
        {
            upgradeProgress = _playerProgress.UpgradeProgresses.FirstOrDefault(progress => progress.Id == upgradeModelId);
            
            return upgradeProgress.Id != default;
        }

        private void OnSetCountWaveCompleted(int waveNumberCompleted) => 
            _waveNumberCompleted = waveNumberCompleted;

        private void OnKeySpawned() => 
            _countKeySpawned++;

        private void OnKeyUsed() => 
            _countKeySpawned--;
    }
}