using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Source.Scripts.Players.PlayerStats;
using UnityEngine;

namespace Source.Scripts.Infrastructure.SaveLoadData
{
    public class SaveLoadService
    {
        private const string SaveFileName = "/Player.data";

        public void SavePlayerProgress(Stats stats)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + SaveFileName;
            FileStream stream = new FileStream(path, FileMode.Create);
            PlayerProgress playerProgress = new PlayerProgress(stats);
            
            formatter.Serialize(stream, playerProgress);
            stream.Close();
        }

        public PlayerProgress LoadPlayerProgress()
        {
            string path = Application.persistentDataPath + SaveFileName;

            if (File.Exists(path) == false)
            {
                Debug.Log("Save file not found in " + path);
            
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