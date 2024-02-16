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
            CurrentValue = upgradeConfig.CurrentValue;
            NextValue = upgradeConfig.NextValue;
            CurrentLevel = upgradeConfig.CurrentLevel;
            NextLevel = upgradeConfig.NextLevel;
            MaxLevel = upgradeConfig.MaxLevel;
        }

        public event Action<UpgradeModel> Changed;
        
        public UpgradeSriptableObject Config { get; set; }
        public StatType StatType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentValue { get; set; }
        public int NextValue { get; set; }
        public int CurrentLevel { get; set; }
        public int NextLevel { get; set; }
        public int MaxLevel { get; set; }
        
        public bool IsUpgadeable => CurrentLevel < MaxLevel;
        
        public float GetNextValue()
        {
            if (CurrentLevel >= MaxLevel)
                return CurrentValue;
            
            return CurrentValue + NextValue;
        }

        public void Upgrade()
        {
            if (IsUpgadeable == false)
                throw new Exception("Upgrade not possible");
            
            CurrentLevel++;
            CurrentValue = CurrentLevel + NextValue;
            Changed?.Invoke(this);
        }

        public void SetLevel(int level)
        {
            CurrentLevel = level;
            CurrentValue = CurrentLevel * NextValue;
        }
    }
}