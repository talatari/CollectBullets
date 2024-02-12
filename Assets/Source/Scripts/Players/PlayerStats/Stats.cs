using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class Stats
    {
        private DamageStats _damageStats;
        private HealthStats _healthStats;
        
        public DamageStats DamageStats => _damageStats;
        public HealthStats HealthStats => _healthStats;

        public Stats(DamageStats damageStats, HealthStats healthStats)
        {
            _damageStats = damageStats ?? throw new ArgumentNullException(nameof(damageStats));
            _healthStats = healthStats ?? throw new ArgumentNullException(nameof(healthStats));
        }
    }
}