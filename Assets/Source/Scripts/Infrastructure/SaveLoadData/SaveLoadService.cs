using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Source.Scripts.Players.PlayerModels;
using UnityEngine;

namespace Source.Scripts.Infrastructure.SaveLoadData
{
    public class SaveLoadService
    {
        private const string SaveFileName = "/Player.data";

        public void SavePlayerProgress()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            // TODO: QUESTION: будет ли работать Application.persistentDataPath на WebGL? или нужно использовать WebApplication?
            string path = Application.persistentDataPath + SaveFileName;
            FileStream stream = new FileStream(path, FileMode.Create);
            PlayerProgress playerProgress = new PlayerProgress();
            
            formatter.Serialize(stream, playerProgress);
            stream.Close();
        }

        public PlayerProgress LoadDefaultPlayerProgress() => 
            LoadPlayerProgress(); // TODO: load from SO?

        public PlayerProgress LoadPlayerProgress()
        {
            string path = Application.persistentDataPath + SaveFileName;

            if (File.Exists(path) == false)
            {
                Debug.Log($"Save file not found in {path}");
            
                return null;
            }
            
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerProgress playerProgress = formatter.Deserialize(stream) as PlayerProgress;

            stream.Close();

            return playerProgress;
        }
    }
}