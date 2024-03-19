using System;
using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using Source.Codebase.Infrastructure.SaveLoadData;
using Source.Codebase.Upgrades;
using UnityEngine;

namespace Source.Codebase.Infrastructure.Services
{
    public class SaveLoadService
    {
        private const string DataKey = "PlayerProgress_Test_1";
        
        private readonly List<UpgradeModel> _upgradeModels;

        private PlayerProgress _playerProgress;
        private string _dataValue;

        public SaveLoadService(List<UpgradeModel> upgradeModels) => 
            _upgradeModels = upgradeModels ?? throw new ArgumentNullException(nameof(upgradeModels));

        public void Init()
        {
            if (PlayerPrefs.HasKey(DataKey))
                return;
            
            CreateNewPlayerProgress();
        }

        public PlayerProgress CreateNewPlayerProgress()
        {
            Debug.Log("+++ CREATE NEW PLAYER PROGRESS");
            
            _playerProgress = new PlayerProgress();
            
            foreach (UpgradeModel upgradeModel in _upgradeModels)
                upgradeModel.ResetProgress();
            
            _playerProgress.SetUpgradeProgresses(
                _upgradeModels.Select(upgradeModel => UpgradeProgress.FromModel(upgradeModel)).ToList());
            
            SavePlayerProgress();

            return _playerProgress;
        }

        public void SavePlayerProgress()
        {
            _dataValue = JsonUtility.ToJson(_playerProgress);
            
#if UNITY_WEBGL && !UNITY_EDITOR
            // Agava.YandexGames.Utility.PlayerPrefs.SetString(DataKey, _dataValue);
            // Agava.YandexGames.Utility.PlayerPrefs.Save();
            //
            // Debug.Log($"+++ SAVE AGAVA DataKey: {DataKey}");
            // Debug.Log($"+++ SAVE AGAVA _dataValue: {_dataValue}");

            PlayerAccount.SetCloudSaveData(_dataValue);
#endif
            Debug.Log($"+++ SAVE DataKey: {DataKey}");
            Debug.Log($"+++ SAVE _dataValue: {_dataValue}");
            
            PlayerPrefs.SetString(DataKey, _dataValue);
            PlayerPrefs.Save();

            _dataValue = "";
        }

        public PlayerProgress LoadPlayerProgress()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerAccount.GetCloudSaveData((data) => _dataValue = data);
            Debug.Log($"+++ _dataValue: {_dataValue}");
            
            // Debug.Log($"+++ Agava.YandexGames.Utility.PlayerPrefs.HasKey(DataKey): {Agava.YandexGames.Utility.PlayerPrefs.HasKey(DataKey)}");
            // Debug.Log($"+++ _dataValue: {_dataValue}");
            //
            // Agava.YandexGames.PlayerAccount.GetCloudSaveData();
            //
            // if (Agava.YandexGames.Utility.PlayerPrefs.HasKey(DataKey))
            //     _dataValue = Agava.YandexGames.Utility.PlayerPrefs.GetString(DataKey);
            //
            // Debug.Log($"+++ LOAD AGAVA DataKey: {DataKey}");
            // Debug.Log($"+++ LOAD AGAVA _dataValue: {_dataValue}");
#endif
            if (string.IsNullOrEmpty(_dataValue))
            {
                _dataValue = PlayerPrefs.GetString(DataKey);
                
                Debug.Log($"+++ LOAD DataKey: {DataKey}");
                Debug.Log($"+++ LOAD _dataValue: {_dataValue}");
            }
            
            _playerProgress = JsonUtility.FromJson<PlayerProgress>(_dataValue);
            
            return _playerProgress;
        }
    }
}