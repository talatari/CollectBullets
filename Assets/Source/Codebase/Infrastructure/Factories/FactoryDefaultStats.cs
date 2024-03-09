using System;
using Source.Codebase.Players.PlayerModels;
using Source.Codebase.SO;

namespace Source.Codebase.Infrastructure.Factories
{
    public class FactoryDefaultStats
    {
        private readonly PlayerScriptableObject _playerConfig;

        public FactoryDefaultStats(PlayerScriptableObject playerConfig) => 
            _playerConfig = playerConfig ? playerConfig : throw new ArgumentNullException(nameof(playerConfig));

        public Stats Create() =>
            new(
                new DamageStats(
                    _playerConfig.Damage, 
                    _playerConfig.Burning,
                    _playerConfig.BurningDuration,
                    _playerConfig.Vampirism, 
                    _playerConfig.ClipCapacity, 
                    _playerConfig.ShootingDelay), 
                new HealthStats(
                    _playerConfig.MaxHealth, 
                    _playerConfig.Regeneration),
                new CommonStats(
                    _playerConfig.Magnet, 
                    _playerConfig.Speed, 
                    _playerConfig.Freeze,
                    _playerConfig.RadiusAttack));
    }
}