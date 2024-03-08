using System;
using Source.Codebase.Players.PlayerModels;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Codebase.Players
{
    public class RadiusEnemyDetectChanger : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        private float _diameter;
        private float _radius;
        private CommonStats _commonStats;

        public float Radius => _radius;

        public void Init(CommonStats commonStats)
        {
            _commonStats = commonStats ?? throw new ArgumentNullException(nameof(commonStats));
            _radius = commonStats.RadiusAttack;
            SetDiameter();
            _commonStats.RadiusAttackChanged += OnSetRadius;
        }

        private void OnDestroy() => 
            _commonStats.RadiusAttackChanged -= OnSetRadius;

        private void OnSetRadius(int radiusAttack)
        {
            _radius = radiusAttack;
            SetDiameter();
        }
        
        private void SetDiameter()
        {
            _diameter = _radius * 2;
            _image.transform.localScale = new Vector3(_diameter, _diameter, _diameter);
        }
    }
}