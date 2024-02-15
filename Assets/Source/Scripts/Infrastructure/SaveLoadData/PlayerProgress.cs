using System;
using Source.Scripts.Players.PlayerStats;

namespace Source.Scripts.Infrastructure.SaveLoadData
{
    [Serializable]
    public class PlayerProgress
    {
        public int Damage;
        public float Burning;
        public float Vampirism;
        public int ClipCapacity;
        public float ShootingDelay;
        public int MaxHealth;
        public float Regenaration;
        public float Magnet;
        public float Speed;
        public float Freeze;
        
        private Stats _stats;

        public PlayerProgress(Stats stats)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));

            Damage = _stats.DamageStats.Damage;
            Burning = _stats.DamageStats.Burning;
            Vampirism = _stats.DamageStats.Vampirism;
            ClipCapacity = _stats.DamageStats.ClipCapacity;
            ShootingDelay = _stats.DamageStats.ShootingDelay;
            
            MaxHealth = _stats.HealthStats.MaxHealth;
            Regenaration = _stats.HealthStats.Regeneration;
            
            Magnet = _stats.CommonStats.Magnet;
            Speed = _stats.CommonStats.Speed;
            Freeze = _stats.CommonStats.Freeze;
        }
    }
}