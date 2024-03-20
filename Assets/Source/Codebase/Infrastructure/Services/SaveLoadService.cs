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
        private bool _isCloudLoaded;
        private bool _isCloudLoadedFailed;

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
            PlayerAccount.SetCloudSaveData(_dataValue);
#endif
            PlayerPrefs.SetString(DataKey, _dataValue);
            PlayerPrefs.Save();

            _dataValue = "";
        }
        
        public PlayerProgress LoadPlayerProgress()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            CloudDataLoaded();
#endif
            LoadLocalPlayerProgress();
            
            _playerProgress = JsonUtility.FromJson<PlayerProgress>(_dataValue);
            
            return _playerProgress;
        }

        private void CloudDataLoaded()
        {
            PlayerAccount.GetCloudSaveData(
                sucessData => OnGetCloudSaveDataSuccess(sucessData), 
                errorData =>  OnGetCloudSaveDataError(errorData));
        }

        private void OnGetCloudSaveDataSuccess(string sucessData)
        {
            Debug.Log($"+++ Callback SUCCESS: {sucessData}");
            _dataValue = sucessData;
        }

        private void OnGetCloudSaveDataError(string errorData)
        {
            Debug.Log($"+++ Callback ERROR: {errorData}");
            LoadLocalPlayerProgress();
        }

        private void LoadLocalPlayerProgress()
        {
            if (string.IsNullOrEmpty(_dataValue))
                _dataValue = PlayerPrefs.GetString(DataKey);
        }
    }
}