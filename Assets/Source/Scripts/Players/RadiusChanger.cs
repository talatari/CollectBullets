using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Players
{
    public class RadiusChanger : MonoBehaviour
    {
        [SerializeField] private Image _radius;
        [SerializeField, Range(0.1f, 0.28f)] private float _value = 0.25f;

        private void OnValidate() => 
            _radius.transform.localScale = new Vector3(_value, _value, _value);

        public void SetRadius(float value)
        {
            if (value <= 0) 
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _value = value;
        }
    }
}