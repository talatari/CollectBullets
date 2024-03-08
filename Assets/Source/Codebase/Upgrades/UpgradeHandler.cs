using System;
using System.Collections.Generic;
using Source.Codebase.Players.PlayerModels;

namespace Source.Codebase.Upgrades
{
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
            {
                SetCurrentValue(upgradeModel);
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
            // TODO: прокинуть сюда ссылку на UpgradeService или на SaveLoadService и сохранять значение upgradeModel.Save(upgradeModel);
            // под капотом сохранить как UpgradeProgress
            switch (upgradeModel.StatType)
            {
                case StatType.Damage:
                    upgradeModel.SetCurrentValue(_stats.DamageStats.AddDamage(upgradeModel.IncrementValue));
                    break;

                case StatType.Burning:
                    upgradeModel.SetCurrentValue(_stats.DamageStats.AddBurning(upgradeModel.IncrementValue));
                    break;

                case StatType.Vampirism:
                    upgradeModel.SetCurrentValue(_stats.DamageStats.AddVampirism(upgradeModel.IncrementValue));
                    break;

                case StatType.ClipCapacity:
                    upgradeModel.SetCurrentValue(_stats.DamageStats.AddClipCapacity(upgradeModel.IncrementValue));
                    break;

                case StatType.ShootingDelay:
                    upgradeModel.SetCurrentValue(_stats.DamageStats.AddShootingDelay(upgradeModel.IncrementValue));
                    break;

                case StatType.MaxHealth:
                    upgradeModel.SetCurrentValue(_stats.HealthStats.AddMaxHealth(upgradeModel.IncrementValue));
                    break;

                case StatType.Regeneration:
                    upgradeModel.SetCurrentValue(_stats.HealthStats.AddRegeneration(upgradeModel.IncrementValue));
                    break;

                case StatType.Magnet:
                    upgradeModel.SetCurrentValue(_stats.CommonStats.AddMagnet(upgradeModel.IncrementValue));
                    break;

                case StatType.Speed:
                    upgradeModel.SetCurrentValue(_stats.CommonStats.AddSpeed(upgradeModel.IncrementValue));
                    break;

                case StatType.Freeze:
                    upgradeModel.SetCurrentValue(_stats.CommonStats.AddFreeze(upgradeModel.IncrementValue));
                    break;
                
                case StatType.RadiusAttack:
                    upgradeModel.SetCurrentValue(_stats.CommonStats.AddRadiusAttack(upgradeModel.IncrementValue));
                    break;
            }
        }
        
        private void SetCurrentValue(UpgradeModel upgradeModel)
        {
            switch (upgradeModel.StatType)
            {
                case StatType.Damage:
                    upgradeModel.SetCurrentValue(_stats.DamageStats.Damage);
                    break;

                case StatType.Burning:
                    upgradeModel.SetCurrentValue(_stats.DamageStats.Burning);
                    break;

                case StatType.Vampirism:
                    upgradeModel.SetCurrentValue(_stats.DamageStats.Vampirism);
                    break;

                case StatType.ClipCapacity:
                    upgradeModel.SetCurrentValue(_stats.DamageStats.ClipCapacity);
                    break;

                case StatType.ShootingDelay:
                    upgradeModel.SetCurrentValue(_stats.DamageStats.ShootingDelay);
                    break;

                case StatType.MaxHealth:
                    upgradeModel.SetCurrentValue(_stats.HealthStats.MaxHealth);
                    break;

                case StatType.Regeneration:
                    upgradeModel.SetCurrentValue(_stats.HealthStats.Regeneration);
                    break;

                case StatType.Magnet:
                    upgradeModel.SetCurrentValue(_stats.CommonStats.Magnet);
                    break;

                case StatType.Speed:
                    upgradeModel.SetCurrentValue(_stats.CommonStats.Speed);
                    break;

                case StatType.Freeze:
                    upgradeModel.SetCurrentValue(_stats.CommonStats.Freeze);
                    break;
                
                case StatType.RadiusAttack:
                    upgradeModel.SetCurrentValue(_stats.CommonStats.RadiusAttack);
                    break;
            }
        }
    }
}