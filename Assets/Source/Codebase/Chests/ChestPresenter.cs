using System;
using Source.Codebase.Players;
using UnityEngine;

namespace Source.Codebase.Chests
{
    public class ChestPresenter : MonoBehaviour
    {
        public event Action KeyDropped;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                if (player.HaveCollectedKey())
                {
                    player.DropKey();
                    KeyDropped?.Invoke();
                    
                    print("Key dropped.");
                }
        }
    }
}