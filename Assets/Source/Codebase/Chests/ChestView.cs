using System;
using Source.Codebase.Chests.Interfaces;
using Source.Codebase.Players;
using UnityEngine;

namespace Source.Codebase.Chests
{
    public class ChestView : MonoBehaviour, IChestView
    {
        public event Action KeyUsed;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                if (player.HaveCollectedKey())
                {
                    player.UseKey();
                    KeyUsed?.Invoke();
                    
                    print("Key dropped.");
                }
        }
    }
}