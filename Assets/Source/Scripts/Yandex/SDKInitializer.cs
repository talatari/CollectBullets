using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Scripts.Yandex
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
            // TODO: remove to MainScene when load all resoureces
            YandexGamesSdk.GameReady();
            
            SceneManager.LoadScene(MainScene);
        }
    }
}