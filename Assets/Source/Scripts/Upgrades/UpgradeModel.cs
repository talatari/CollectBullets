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
            CurrentLevel = upgradeConfig.Level;
            MaxLevel = upgradeConfig.MaxLevel;
            IncrementValue = upgradeConfig.IncrementValue;
        }

        public event Action<UpgradeModel> Changed;
        
        public UpgradeSriptableObject Config { get; set; }
        public StatType StatType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public int CurrentLevel { get; set; }
        public int MaxLevel { get; set; }
        public int IncrementValue { get; set; }
        public bool IsUpgadeable => CurrentLevel < MaxLevel;
        
        public int GetNextValue()
        {
            if (CurrentLevel >= MaxLevel)
                return Value;
            
            return Value + IncrementValue;
        }

        public void Upgrade()
        {
            if (IsUpgadeable == false)
                throw new Exception("Upgrade not possible");
            
            CurrentLevel++;
            Value = CurrentLevel * IncrementValue;
            Changed?.Invoke(this);
        }

        public void SetLevel(int level)
        {
            CurrentLevel = level;
            Value = CurrentLevel * IncrementValue;
        }
    }
}