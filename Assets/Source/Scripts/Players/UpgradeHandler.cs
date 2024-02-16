using System;
using System.Collections.Generic;
using Source.Scripts.Players.PlayerModels;
using Source.Scripts.Upgrades;

namespace Source.Scripts.Players
{
    public class UpgradeHandler : IDisposable
    {
        private readonly Stats _stats;
        private readonly List<UpgradeModel> _upgradeModels;

        public UpgradeHandler(Stats stats, List<UpgradeModel> upgradeModels)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
            _upgradeModels = upgradeModels ?? throw new ArgumentNullException(nameof(upgradeModels));
        }
        
        public void UpdateStats()
        {
            foreach (UpgradeModel upgradeModel in _upgradeModels)
            {
                ApplyUpgrade(upgradeModel);
                upgradeModel.Changed += ApplyUpgrade;
            }
        }

        public void Dispose()
        {
            foreach (UpgradeModel upgradeModel in _upgradeModels)
                upgradeModel.Changed -= ApplyUpgrade;
        }

        private void ApplyUpgrade(UpgradeModel upgradeModel)
        {
            if (upgradeModel.Value == 0)
                return;
            
            switch (upgradeModel.StatType)
            {
                case StatType.Damage:
                    _stats.DamageStats.AddDamage(upgradeModel.Value);
                    break;
                
                case StatType.Burning:
                    _stats.DamageStats.AddBurning(upgradeModel.Value);
                    break;
                
                case StatType.Vampirism:
                    _stats.DamageStats.AddVampirism(upgradeModel.Value);
                    break;
                
                case StatType.ClipCapacity:
                    _stats.DamageStats.AddClipCapacity(upgradeModel.Value);
                    break;
                
                case StatType.ShootingDelay:
                    _stats.DamageStats.AddShootingDelay(upgradeModel.Value);
                    break;
                
                case StatType.MaxHealth:
                    _stats.HealthStats.AddMaxHealth(upgradeModel.Value);
                    break;
                
                case StatType.Regeneration:
                    _stats.HealthStats.AddRegeneration(upgradeModel.Value);
                    break;
                
                case StatType.Magnet:
                    _stats.CommonStats.AddMagnet(upgradeModel.Value);
                    break;
                
                case StatType.Speed:
                    _stats.CommonStats.AddSpeed(upgradeModel.Value);
                    break;
                
                case StatType.Freeze:
                    _stats.CommonStats.AddFreeze(upgradeModel.Value);
                    break;
            }
        }
    }
}