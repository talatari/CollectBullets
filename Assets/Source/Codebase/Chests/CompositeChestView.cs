using System;
using Source.Codebase.Chests.Interfaces;
using Source.Codebase.Infrastructure.Services;
using UnityEngine;

namespace Source.Codebase.Chests
{
    public class CompositeChestView : MonoBehaviour, IChestView
    {
        [SerializeField] private ChestView[] _chestViews;

        private TargetProvider _targetProvider;

        public event Action KeyUsed;

        public void Init(TargetProvider targetProvider) => 
            _targetProvider = targetProvider ?? throw new ArgumentNullException(nameof(targetProvider));

        private void Start()
        {
            foreach (ChestView chestView in _chestViews)
            {
                chestView.SetTarget(_targetProvider.Target);
                chestView.KeyUsed += OnKeyUsed;
            }
        }

        private void OnDestroy()
        {
            foreach (ChestView chestView in _chestViews)
                chestView.KeyUsed -= OnKeyUsed;
        }

        public void CollectKey()
        {
            foreach (ChestView chestView in _chestViews)
                chestView.CollectKey();
        }

        public void UseKey()
        {
            foreach (ChestView chestView in _chestViews)
                chestView.UseKey();
        }

        private void OnKeyUsed()
        {
            foreach (ChestView chestView in _chestViews)
                chestView.UseKey();
            
            KeyUsed?.Invoke();
        }
    }
}