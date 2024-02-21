using System;
using Source.Scripts.Players.PlayerModels;
using Source.Scripts.Players.Weapons;
using UnityEngine;

namespace Source.Scripts.Players.CollisionHandlers
{
    public class CollisionForBullets : CollisionHandler
    {
        private const float RatioIncrement = 0.5f;
        
        [SerializeField] private LayerMask _bulletLayer;

        private Collider[] _bulletColliders = new Collider[MaxOverlap];
        private WeaponHandler _weaponHandler;
        private CommonStats _commonStats;
        private float _radiusPickUpBullets;
        private int _baseMagnet;
        private bool _isInit;

        public event Action BulletCollected;
        
        public void Init(WeaponHandler weaponHandler, int magnet)
        {
            _weaponHandler = weaponHandler ? weaponHandler : throw new ArgumentNullException(nameof(weaponHandler));
            
            if (magnet <= 0) 
                throw new ArgumentOutOfRangeException(nameof(magnet));

            _baseMagnet = magnet;
            _radiusPickUpBullets = _baseMagnet;

            _isInit = true;
        }

        private void Update()
        {
            if (_isInit == false)
                return;
            
            OverlapBullets();
        }

        public void SetMagnet(int magnet)
        {
            if (magnet <= 0)
                throw new ArgumentOutOfRangeException(nameof(magnet));

            float newMagnet = _baseMagnet + (magnet - _baseMagnet) * RatioIncrement;
            
            _radiusPickUpBullets = newMagnet;
        }

        private void OverlapBullets()
        {
            if (_weaponHandler.CollectedBullets >= _weaponHandler.ClipCapacity)
                return;
            
            int bulletsAmount = Physics.OverlapSphereNonAlloc(
                transform.position, _radiusPickUpBullets, _bulletColliders, _bulletLayer);

            for (int i = 0; i < bulletsAmount; i++)
                if (_bulletColliders[i].TryGetComponent(out Bullet bullet))
                {
                    bullet.OnReleaseToPool();
                    BulletCollected?.Invoke();
                }
        }
    }
}