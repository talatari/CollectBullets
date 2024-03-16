using System.IO;
using Source.Codebase.Infrastructure.SaveLoadData;
using UnityEngine;
using Newtonsoft.Json;

namespace Source.Codebase.Infrastructure.Services
{
    public class SaveLoadService
    {
        private PlayerProgress _playerProgress = new();
        private string _pathToSave = Application.persistentDataPath + "/Player.json";

        public void SavePlayerProgress()
        {
            string json = JsonConvert.SerializeObject(_playerProgress);
            File.WriteAllText(_pathToSave, json);
        }

        public PlayerProgress LoadPlayerProgress()
        {
            if (File.Exists(_pathToSave) == false)
            {
                Debug.Log($"Save file not found in {_pathToSave}");
                
                return _playerProgress;
            }
            
            string json = File.ReadAllText(_pathToSave);
            _playerProgress = JsonConvert.DeserializeObject<PlayerProgress>(json);
            
            return _playerProgress;
        }
    }
}