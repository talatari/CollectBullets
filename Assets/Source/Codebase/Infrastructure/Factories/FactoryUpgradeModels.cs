using System;
using System.Collections.Generic;
using Source.Codebase.SO;
using Source.Codebase.Upgrades;

namespace Source.Codebase.Infrastructure.Factories
{
    public class FactoryUpgradeModels
    {
        private readonly UpgradeSriptableObject[] _upgradeConfigs;
        private List<UpgradeModel> _upgradeModels;

        public FactoryUpgradeModels(UpgradeSriptableObject[] upgradeConfigs) => 
            _upgradeConfigs = upgradeConfigs ?? throw new ArgumentNullException(nameof(upgradeConfigs));

        public List<UpgradeModel> Create()
        {
            _upgradeModels = new List<UpgradeModel>();

            foreach (UpgradeSriptableObject upgradeConfig in _upgradeConfigs)
                _upgradeModels.Add(new UpgradeModel(upgradeConfig));

            return _upgradeModels;
        }
    }
}