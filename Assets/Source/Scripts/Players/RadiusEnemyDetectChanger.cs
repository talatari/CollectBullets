using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Players
{
    public class RadiusEnemyDetectChanger : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField, Range(5f, 20f)] private float _diameter = 8f;

        private float _radius;
        
        public float Radius => _radius;
        
        private void OnValidate()
        {
            if (_image == null)
                return;
            
            _image.transform.localScale = new Vector3(_diameter, _diameter, _diameter);
            _radius = _diameter / 2;
        }

        public void SetRadius(float value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _diameter = value;
        }
    }
}