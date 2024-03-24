using System;
using Source.Codebase.Bullets;
using Source.Codebase.Players.Bug;
using Source.Codebase.Players.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Codebase.Players.CollisionHandlers
{
    public class CollisionForBullets : CollisionHandler
    {
        private const float RatioIncrement = 0.35f;
        
        [SerializeField] private LayerMask _bulletLayer;
        [SerializeField] private Image _rangePickUpBulletsImage;
        
        private Collider[] _bulletColliders = new Collider[MaxOverlap];
        private Bag _bag;
        private float _radiusPickUp;
        private int _baseRadiusMagnet;
        private bool _isInit;

        public event Action BulletCollected;
        
        public void Init(Bag bag, int magnet)
        {
            _bag = bag ? bag : throw new ArgumentNullException(nameof(bag));
            if (magnet <= 0) 
                throw new ArgumentOutOfRangeException(nameof(magnet));

            _baseRadiusMagnet = magnet;
            _radiusPickUp = _baseRadiusMagnet;
            
            _isInit = true;
            
            SetupPickUpBulletsDiameter();
        }

        private void Update()
        {
            if (_isInit == false)
                return;
            
            if (_bag.HaveFreeSlot == false)
                return;
            
            OverlapBullets();
        }

        public void SetRadiusPickUp(int radius)
        {
            if (radius <= 0)
                throw new ArgumentOutOfRangeException(nameof(radius));

            float radiusPickUp = _baseRadiusMagnet + (radius - _baseRadiusMagnet) * RatioIncrement;
            _radiusPickUp = radiusPickUp;
            
            SetupPickUpBulletsDiameter();
        }

        private void SetupPickUpBulletsDiameter()
        {
            if (_rangePickUpBulletsImage == null)
                return;
            
            float diameterPickUp = _radiusPickUp * 2;
            _rangePickUpBulletsImage.transform.localScale = new Vector3(diameterPickUp, diameterPickUp, diameterPickUp);
        }

        private void OverlapBullets()
        {
            int bulletsAmount = Physics.OverlapSphereNonAlloc(
                transform.position, _radiusPickUp, _bulletColliders, _bulletLayer);

            for (int i = 0; i < bulletsAmount; i++)
                if (_bulletColliders[i].TryGetComponent(out Bullet bullet))
                {
                    bullet.OnReleaseToPool();
                    BulletCollected?.Invoke();
                }
        }
    }
}