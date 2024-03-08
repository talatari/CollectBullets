using System;

namespace Source.Codebase.Players.PlayerModels
{
    [Serializable]
    public class Stats
    {
        private DamageStats _damageStats;
        private HealthStats _healthStats;
        private CommonStats _commonStats;
        
        public DamageStats DamageStats => _damageStats;
        public HealthStats HealthStats => _healthStats;
        public CommonStats CommonStats => _commonStats;

        public Stats(DamageStats damageStats, HealthStats healthStats, CommonStats commonStats)
        {
            _damageStats = damageStats ?? throw new ArgumentNullException(nameof(damageStats));
            _healthStats = healthStats ?? throw new ArgumentNullException(nameof(healthStats));
            _commonStats = commonStats ?? throw new ArgumentNullException(nameof(commonStats));
        }
    }
}