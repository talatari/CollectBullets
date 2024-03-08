using System;
using Source.Codebase.SO;

namespace Source.Codebase.Upgrades
{
    public class UpgradeModel
    {
        public UpgradeModel(UpgradeSriptableObject upgradeConfig)
        {
            Config = upgradeConfig;
            StatType = upgradeConfig.StatType;
            Id = upgradeConfig.Id;
            Name = upgradeConfig.Name;
            CurrentValue = upgradeConfig.CurrentValue;
            IncrementValue = upgradeConfig.IncrementValue;
            CurrentLevel = upgradeConfig.CurrentLevel;
            MaxLevel = upgradeConfig.MaxLevel;
        }

        public event Action<UpgradeModel> Changed;
        
        public UpgradeSriptableObject Config { get; set; }
        public StatType StatType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentValue { get; set; }
        public int IncrementValue { get; set; }
        public int CurrentLevel { get; set; }
        public int MaxLevel { get; set; }
        
        public bool IsUpgradeable => CurrentLevel <= MaxLevel;
        
        public void Upgrade()
        {
            if (IsUpgradeable == false)
                throw new Exception("Upgrade not possible");
            
            CurrentLevel++;
            Changed?.Invoke(this);
        }

        public void SetCurrentValue(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            CurrentValue = value;
        }
    }
}