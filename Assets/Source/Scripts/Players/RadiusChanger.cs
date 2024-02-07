using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Players
{
    public class RadiusChanger : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField, Range(0.1f, 0.3f)] private float _radius = 0.2f;
        
        public float Radius => _radius;
        
        private void OnValidate()
        {
            if (_image == null)
                return;
            
            _image.transform.localScale = new Vector3(_radius, _radius, _radius);
        }

        public void SetRadius(float value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _radius = value;
        }
    }
}