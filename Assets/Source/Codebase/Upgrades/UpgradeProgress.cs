using System;

namespace Source.Codebase.Upgrades
{
    [Serializable]
    public struct UpgradeProgress
    {
        public static UpgradeProgress FromModel(UpgradeModel upgradeModel)
        {
            if (upgradeModel == null) 
                throw new ArgumentNullException(nameof(upgradeModel));

            return new UpgradeProgress(upgradeModel.Id, upgradeModel.CurrentLevel);
        }

        public UpgradeProgress(int id, int currentLevel)
        {
            if (id <= 0) 
                throw new ArgumentOutOfRangeException(nameof(id));
            if (currentLevel <= 0) 
                throw new ArgumentOutOfRangeException(nameof(currentLevel));

            Id = id;
            CurrentLevel = currentLevel;
        }

        public int Id { get; private set; }
        public int CurrentLevel { get; private set; }
    }
}