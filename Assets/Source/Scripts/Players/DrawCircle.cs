using UnityEngine;

namespace Source.Scripts.Players
{
    public class DrawCircle : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Player _player;
        [SerializeField] private float _heightCircle = 0.15f;
        [SerializeField] private int _steps = 100;
        [SerializeField] private int _radius = 10;
        
        private void Update()
        {
            Draw();
        }

        private void Draw()
        {
            _lineRenderer.positionCount = _steps;

            for (int currentStep = 0; currentStep < _steps; currentStep++)
            {
                float circumferenceProgress = (float)currentStep / _steps;
                float currentRadian = circumferenceProgress * 2 * Mathf.PI;
                float xScaled = Mathf.Cos(currentRadian);
                float zScaled = Mathf.Sin(currentRadian);
                float x = xScaled * _radius;
                float z = zScaled * _radius;
                Vector3 currentPosition = new Vector3(
                    _player.transform.position.x + x, _heightCircle, _player.transform.position.z + z);
                _lineRenderer.SetPosition(currentStep, currentPosition);
            }
        }
    }
}