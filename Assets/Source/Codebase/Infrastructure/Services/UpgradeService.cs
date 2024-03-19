using System;
using System.Collections.Generic;
using System.Linq;
using Source.Codebase.Upgrades;

namespace Source.Codebase.Infrastructure.Services
{
    public class UpgradeService
    {
        private readonly ProgressService _progressService;
        private List<UpgradeModel> _upgradeModels;

        public UpgradeService(ProgressService progressService, List<UpgradeModel> upgradeModels)
        {
            _progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
            _upgradeModels = upgradeModels ?? throw new ArgumentNullException(nameof(upgradeModels));
        }

        public void Init()
        {
            foreach (UpgradeModel upgradeModel in _upgradeModels)
                if (_progressService.TryGetUpgradeProgress(upgradeModel.Id, out UpgradeProgress upgradeProgress))
                    upgradeModel.UpgradeTo(upgradeProgress.CurrentLevel);
        }

        public void Upgrade(int id)
        {
            _upgradeModels.First(model => model.Id == id).Upgrade();

            _progressService.SaveUpgrades(_upgradeModels);
        }

        public bool TryGetUpgradeableModels(out List<UpgradeModel> upgradeModels)
        {
            upgradeModels = _upgradeModels.Where(model => model.IsUpgradeable).ToList();

            return upgradeModels != null;
        }
    }
}