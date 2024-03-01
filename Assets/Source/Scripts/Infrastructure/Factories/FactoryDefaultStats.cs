using System;
using Source.Scripts.Players.PlayerModels;
using Source.Scripts.SO;

namespace Source.Scripts.Infrastructure.Factories
{
    public class FactoryDefaultStats
    {
        private readonly PlayerScriptableObject _playerScriptableObject;

        public FactoryDefaultStats(PlayerScriptableObject playerScriptableObject)
        {
            _playerScriptableObject = playerScriptableObject
                ? playerScriptableObject
                : throw new ArgumentNullException(nameof(playerScriptableObject));
        }
        
        public Stats Create() =>
            new(
                new DamageStats(
                    _playerScriptableObject.Damage, 
                    _playerScriptableObject.Burning,
                    _playerScriptableObject.BurningDuration,
                    _playerScriptableObject.Vampirism, 
                    _playerScriptableObject.ClipCapacity, 
                    _playerScriptableObject.ShootingDelay), 
                new HealthStats(
                    _playerScriptableObject.MaxHealth, 
                    _playerScriptableObject.Regeneration),
                new CommonStats(
                    _playerScriptableObject.Magnet, 
                    _playerScriptableObject.Speed, 
                    _playerScriptableObject.Freeze,
                    _playerScriptableObject.RadiusAttack));
    }
}