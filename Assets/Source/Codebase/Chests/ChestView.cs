using System;
using Source.Codebase.Chests.Interfaces;
using Source.Codebase.Players;
using UnityEngine;

namespace Source.Codebase.Chests
{
    public class ChestView : MonoBehaviour, IChestView
    {
        [SerializeField] private ChestPointer _chestPointer;
        [SerializeField] private float _radius;

        public event Action KeyUsed;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                if (player.HaveCollectedKey())
                {
                    player.UseKey();
                    KeyUsed?.Invoke();
                }
        }

        public void SetTarget(Transform player)
        {
            if (player == null) 
                throw new ArgumentNullException(nameof(player));
            if (_chestPointer == null)
                throw new ArgumentNullException(nameof(_chestPointer));
            
            _chestPointer.SetTarget(player);
            _chestPointer.SetRadius(_radius);
        }

        public void CollectKey() => 
            _chestPointer.CollectKey();
        
        public void UseKey() => 
            _chestPointer.UseKey();
    }
}