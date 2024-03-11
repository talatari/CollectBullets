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
            new (
                new DamageStats(_playerConfig), 
                new HealthStats(_playerConfig), 
                new CommonStats(_playerConfig));
    }
}