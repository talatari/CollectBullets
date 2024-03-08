using UnityEngine;
using UnityEngine.UI;

namespace Source.Codebase.GameOver
{
    public class RestartGameView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [field: SerializeField] public Button RestartButton { get; private set; }

        public void Show() => 
            _canvas.enabled = true;

        public void Hide() => 
            _canvas.enabled = false;
    }
}