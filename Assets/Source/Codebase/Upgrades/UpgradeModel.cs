using System;
using Source.Codebase.SO;

namespace Source.Codebase.Upgrades
{
    public class UpgradeModel
    {
        public UpgradeModel(UpgradeSriptableObject upgradeConfig)
        {
            Config = upgradeConfig;
            StatType = Config.StatType;
            Id = Config.Id;
            Name = Config.Name;
            CurrentValue = Config.CurrentValue;
            IncrementValue = Config.IncrementValue;
            CurrentLevel = Config.CurrentLevel;
            MaxLevel = Config.MaxLevel;
        }

        public event Action<UpgradeModel> Changed;
        
        public UpgradeSriptableObject Config { get; }
        public StatType StatType { get; }
        public int Id { get; }
        public string Name { get; }
        public int CurrentValue { get; set; }
        public int IncrementValue { get; }
        public int CurrentLevel { get; set; }
        public int MaxLevel { get; }
        
        public bool IsUpgradeable => CurrentLevel <= MaxLevel;
        
        public void Upgrade()
        {
            if (IsUpgradeable == false)
                throw new Exception("Upgrade not possible");
            
            CurrentLevel++;
            CurrentValue += IncrementValue;
            Changed?.Invoke(this);
        }

        public void ResetProgress()
        {
            CurrentValue = Config.CurrentValue;
            CurrentLevel = Config.CurrentLevel;
            Changed?.Invoke(this);
        }

        public void UpgradeTo(int upgradeLevel)
        {
            CurrentLevel = upgradeLevel;
            CurrentValue = (CurrentLevel - 1) * IncrementValue;
            Changed?.Invoke(this);
        }
    }
}