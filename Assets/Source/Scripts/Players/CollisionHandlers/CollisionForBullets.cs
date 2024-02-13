using System;
using Source.Scripts.Players.PlayerStats;
using Source.Scripts.Players.Weapons;
using UnityEngine;

namespace Source.Scripts.Players.CollisionHandlers
{
    public class CollisionForBullets : CollisionHandler
    {
        [SerializeField] private LayerMask _bulletLayer;

        private Collider[] _bulletColliders = new Collider[MaxOverlap];
        private WeaponHandler _weaponHandler;
        private CommonStats _commonStats;
        private float _radiusPickUpBullets;
        private int _collectedBullets;
        private int _clipCapacity;
        private bool _isInit;

        public event Action BulletCollected;
        
        public void Init(WeaponHandler weaponHandler, CommonStats commonStats)
        {
            _weaponHandler = weaponHandler ? weaponHandler : throw new ArgumentNullException(nameof(weaponHandler));
            _commonStats = commonStats ?? throw new ArgumentNullException(nameof(commonStats));
            
            _radiusPickUpBullets = _commonStats.Magnet;
            _collectedBullets = _weaponHandler.CollectedBullets;
            _clipCapacity = _weaponHandler.ClipCapacity;
            _commonStats.MagnetChanged += OnMagnetChanged;

            _isInit = true;
        }

        private void Update()
        {
            if (_isInit == false)
                return;
            
            OverlapBullets();
        }

        private void OnDisable()
        {
            if (_isInit == false)
                return;
            
            _commonStats.MagnetChanged -= OnMagnetChanged;
        }

        private void OnMagnetChanged(float newMagnet) => 
            _radiusPickUpBullets = newMagnet;

        private void OverlapBullets()
        {
            if (_collectedBullets >= _clipCapacity)
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