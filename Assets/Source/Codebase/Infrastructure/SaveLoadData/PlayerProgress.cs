using System;
using System.Collections.Generic;
using Source.Codebase.Upgrades;

namespace Source.Codebase.Infrastructure.SaveLoadData
{
    [Serializable]
    public class PlayerProgress
    {
        public List<UpgradeProgress> UpgradeProgresses { get; private set; } = new ();
        public int CountWaveCompleted { get; private set; }
        public int CountKeySpawned { get; private set; }

        public void SetUpgradeProgresses(List<UpgradeProgress> upgradeProgresses) => 
            UpgradeProgresses = upgradeProgresses ?? throw new ArgumentNullException(nameof(upgradeProgresses));

        public void SetCountWaveCompleted(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            CountWaveCompleted = value;
        }

        public void SetCountKeySpawned(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            CountKeySpawned = value;
        }
    }
}