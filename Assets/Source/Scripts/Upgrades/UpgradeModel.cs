using System;
using Source.Scripts.SO;

namespace Source.Scripts.Upgrades
{
    public class UpgradeModel
    {
        public UpgradeModel(UpgradeSriptableObject upgradeConfig)
        {
            Config = upgradeConfig;
            StatType = upgradeConfig.StatType;
            Id = upgradeConfig.Id;
            Name = upgradeConfig.Name;
            Value = upgradeConfig.Value;
            IncrementValue = upgradeConfig.IncrementValue;
            CurrentLevel = upgradeConfig.CurrentLevel;
            MaxLevel = upgradeConfig.MaxLevel;
        }

        public event Action<UpgradeModel> Changed;
        
        public UpgradeSriptableObject Config { get; set; }
        public StatType StatType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public float IncrementValue { get; set; }
        public int CurrentLevel { get; set; }
        public int MaxLevel { get; set; }
        
        public bool IsUpgradeable => CurrentLevel < MaxLevel;
        
        public void Upgrade()
        {
            if (IsUpgradeable == false)
                throw new Exception("Upgrade not possible");
            
            CurrentLevel++;
            Value += IncrementValue;
            Changed?.Invoke(this);
        }

        public void SetLevel(int level)
        {
            CurrentLevel = level;
            Value = CurrentLevel * IncrementValue;
        }
    }
}