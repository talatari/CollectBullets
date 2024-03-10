using TMPro;
using UnityEngine;

namespace Source.Codebase.Build
{
    public class BuildDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textBuildNumber;

        private ResourceRequest _request;
        
        private void Awake()
        {
            if (_textBuildNumber.TryGetComponent(out TMP_Text textBuildLabel))
                _textBuildNumber = textBuildLabel;

            _request = Resources.LoadAsync("Build", typeof(BuildScriptableObject));
            _request.completed += Request_completed;
        }

        private void OnDestroy() => 
            _request.completed -= Request_completed;

        private void Request_completed(AsyncOperation obj)
        {
            BuildScriptableObject buildScriptableObject = ((ResourceRequest)obj).asset as BuildScriptableObject;

            if (buildScriptableObject == null)
                Debug.LogError("Build scriptable object not found in resources directory! Check build log for errors!");
            else
                _textBuildNumber.text = $"Version: {Application.version}.{buildScriptableObject.BuildNumber}";
        }
    }
}