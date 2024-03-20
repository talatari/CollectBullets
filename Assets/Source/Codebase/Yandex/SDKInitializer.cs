using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Codebase.Yandex
{
    public class SDKInitializer : MonoBehaviour
    {
        private const string MainScene = "Source/Scenes/Main_Scene";
        
        private void Awake() => 
            YandexGamesSdk.CallbackLogging = true;

        private IEnumerator Start()
        {
            yield return YandexGamesSdk.Initialize(OnInitialized);
        }

        private void OnInitialized() => 
            SceneManager.LoadScene(MainScene);
    }
}