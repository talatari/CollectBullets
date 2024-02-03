using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Scripts
{
    public class SDKInitializer : MonoBehaviour
    {
        private const string MainScene = "Scenes/MainScene";
        
        private void Awake() => 
            YandexGamesSdk.CallbackLogging = true;

        private IEnumerator Start()
        {
            yield return YandexGamesSdk.Initialize(OnInitialized);
        }

        private void OnInitialized()
        {
            YandexGamesSdk.GameReady();
            
            SceneManager.LoadScene(MainScene);
        }
    }
}