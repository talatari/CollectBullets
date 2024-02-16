using System;
using System.Collections.Generic;
using Source.Scripts.SO;
using Source.Scripts.Upgrades;

namespace Source.Scripts.Infrastructure.Factories
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