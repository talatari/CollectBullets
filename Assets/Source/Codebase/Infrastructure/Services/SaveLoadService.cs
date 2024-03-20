using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            LoadLocalPlayerProgress();
            
            _playerProgress = JsonUtility.FromJson<PlayerProgress>(_dataValue);
            
            return _playerProgress;
        }

        private async Task CloudLoadSync()
        {
            bool cloudDataLoaded = false;
            
            PlayerAccount.GetCloudSaveData(
                (data) => { OnGetCloudSaveDataSuccess(data); cloudDataLoaded = true; }, 
                (errorData) => { OnGetCloudSaveDataError(errorData); cloudDataLoaded = true; });
            
            await Task.Run(() => cloudDataLoaded);
        }

        private void OnGetCloudSaveDataSuccess(string data)
        {
            Debug.Log($"+++ Callback SUCCESS: {data}");
            _dataValue = data;
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