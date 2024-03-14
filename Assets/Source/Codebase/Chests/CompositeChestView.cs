using System;
using Source.Codebase.Chests.Interfaces;
using UnityEngine;

namespace Source.Codebase.Chests
{
    public class CompositeChestView : MonoBehaviour, IChestView
    {
        [SerializeField] private ChestView[] _chestViews;
        
        public event Action KeyUsed;

        private void Start()
        {
            foreach (ChestView chestView in _chestViews)
                chestView.KeyUsed += OnKeyUsed;
        }

        private void OnDestroy()
        {
            foreach (ChestView chestView in _chestViews)
                chestView.KeyUsed -= OnKeyUsed;
        }

        private void OnKeyUsed() => 
            KeyUsed?.Invoke();
    }
}