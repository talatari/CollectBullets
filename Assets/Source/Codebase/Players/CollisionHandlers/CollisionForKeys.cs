using System;
using Source.Codebase.Keys;
using Source.Codebase.Players.Bug;
using UnityEngine;

namespace Source.Codebase.Players.CollisionHandlers
{
    public class CollisionForKeys : CollisionHandler
    {
        [SerializeField] private LayerMask _keyLayer;

        private Collider[] _keyColliders = new Collider[MaxOverlap];
        private float _radiusPickUp;
        private bool _isKeyCollected;
        private bool _isInit;
        private Bag _bag;

        public bool IsKeyCollected => _isKeyCollected;
        
        public event Action KeyCollected;

        public void Init(Bag bag, float radiusPickUp)
        {
            _bag = bag ? bag : throw new ArgumentNullException(nameof(bag));
            if (radiusPickUp <= 0) 
                throw new ArgumentOutOfRangeException(nameof(radiusPickUp));

            _radiusPickUp = radiusPickUp;
            _isInit = true;
        }
        
        private void Update()
        {
            if (_isInit == false)
                return;
            
            if (_bag.HaveFreeSlot == false)
                return;
            
            if (_isKeyCollected)
                return;
            
            OverlapBullets();
        }

        public void SetRadiusPickUp(int radiusPickUp)
        {
            if (radiusPickUp <= 0)
                throw new ArgumentOutOfRangeException(nameof(radiusPickUp));

            _radiusPickUp = radiusPickUp;
        }

        public void UseKey() => 
            _isKeyCollected = false;

        private void OverlapBullets()
        {
            int bulletsAmount = Physics.OverlapSphereNonAlloc(
                transform.position, _radiusPickUp, _keyColliders, _keyLayer);

            for (int i = 0; i < bulletsAmount; i++)
                if (_keyColliders[i].TryGetComponent(out Key key))
                {
                    key.OnReleaseToPool();
                    _isKeyCollected = true;
                    KeyCollected?.Invoke();
                }
        }
    }
}