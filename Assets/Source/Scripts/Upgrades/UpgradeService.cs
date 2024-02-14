using System;
using Source.Scripts.Infrastructure.SaveLoadData;
using Source.Scripts.Players.PlayerStats;

namespace Source.Scripts.Upgrades
{
    public class UpgradeService
    {
        private SaveLoadService _saveLoadService;

        public void Init(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService ?? throw new ArgumentNullException(nameof(saveLoadService));
        }

        public PlayerProgress Load() => 
            _saveLoadService.LoadPlayerProgress();
        
        public void Save(Stats stats)
        {
            if (stats == null)
                throw new ArgumentNullException(nameof(stats));
            
            _saveLoadService.SavePlayerProgress(stats);
        }
    }
}