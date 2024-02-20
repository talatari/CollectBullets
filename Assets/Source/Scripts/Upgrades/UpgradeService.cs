using System;
using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Infrastructure.SaveLoadData;

namespace Source.Scripts.Upgrades
{
    public class UpgradeService
    {
        private readonly SaveLoadService _saveLoadService;
        private readonly List<UpgradeModel> _upgradeModels;

        public UpgradeService(SaveLoadService saveLoadService, List<UpgradeModel> upgradeModels)
        {
            _saveLoadService = saveLoadService ?? throw new ArgumentNullException(nameof(saveLoadService));
            _upgradeModels = upgradeModels ?? throw new ArgumentNullException(nameof(upgradeModels));
        }

        public bool TryGetUpgradeableModels(out List<UpgradeModel> upgradeModels)
        {
            upgradeModels = _upgradeModels.Where(model => model.IsUpgradeable).ToList();

            return upgradeModels != null;
        }

        public PlayerProgress LoadDefaultPlayerProgress() => 
            _saveLoadService.LoadDefaultPlayerProgress();

        public PlayerProgress LoadPlayerProgress() => 
            _saveLoadService.LoadPlayerProgress();

        public void SavePlayerProgress() => 
            _saveLoadService.SavePlayerProgress();

        public void Upgrade(int id) => 
            _upgradeModels.First(model => model.Id == id).Upgrade();
    }
}