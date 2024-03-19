using System;
using System.Collections.Generic;
using Source.Codebase.Players.PlayerModels;

namespace Source.Codebase.Upgrades
{
    [Serializable]
    public class UpgradeHandler : IDisposable
    {
        private Stats _stats;
        private List<UpgradeModel> _upgradeModels;

        public UpgradeHandler(Stats stats, List<UpgradeModel> upgradeModels)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
            _upgradeModels = upgradeModels ?? throw new ArgumentNullException(nameof(upgradeModels));
        }
        
        public void Init()
        {
            foreach (UpgradeModel upgradeModel in _upgradeModels)
                upgradeModel.Changed += ApplyUpgrade;
        }

        public void Dispose()
        {
            foreach (UpgradeModel upgradeModel in _upgradeModels)
                upgradeModel.Changed -= ApplyUpgrade;
        }

        public void SetDefaultValues()
        {
            _stats.DamageStats.SetDefaultValues();
            _stats.HealthStats.SetDefaultValues();
            _stats.CommonStats.SetDefaultValues();
        }
        
        private void ApplyUpgrade(UpgradeModel upgradeModel)
        {
            switch (upgradeModel.StatType)
            {
                case StatType.Damage:
                    _stats.DamageStats.AddDamage(upgradeModel.CurrentValue);
                    break;

                case StatType.Burning:
                    _stats.DamageStats.AddBurning(upgradeModel.CurrentValue);
                    break;

                case StatType.Vampirism:
                    _stats.DamageStats.AddVampirism(upgradeModel.CurrentValue);
                    break;

                case StatType.ClipCapacity:
                    _stats.DamageStats.AddClipCapacity(upgradeModel.CurrentValue);
                    break;

                case StatType.ShootingDelay:
                    _stats.DamageStats.AddShootingDelay(upgradeModel.CurrentValue);
                    break;

                case StatType.MaxHealth:
                    _stats.HealthStats.AddMaxHealth(upgradeModel.CurrentValue);
                    break;

                case StatType.Regeneration:
                    _stats.HealthStats.AddRegeneration(upgradeModel.CurrentValue);
                    break;

                case StatType.Magnet:
                    _stats.CommonStats.AddMagnet(upgradeModel.CurrentValue);
                    break;

                case StatType.Speed:
                    _stats.CommonStats.AddSpeed(upgradeModel.CurrentValue);
                    break;

                case StatType.Freeze:
                    _stats.CommonStats.AddFreeze(upgradeModel.CurrentValue);
                    break;
                
                case StatType.RadiusAttack:
                    _stats.CommonStats.AddRadiusAttack(upgradeModel.CurrentValue);
                    break;
            }
        }
    }
}