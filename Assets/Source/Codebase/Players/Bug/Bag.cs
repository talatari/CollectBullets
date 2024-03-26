using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Source.Codebase.Players.Bug
{
    public class Bag : MonoBehaviour
    {
        private const string MaxLabelText = "MAX";
        
        [SerializeField] private Collected _collectedPrefab;
        [SerializeField] private TextMeshProUGUI _maxLabel;
        [SerializeField] private Transform _container;
        
        private float _offsetY = 0.35f;
        private List<Collected> _collected = new();
        private int _activateCollected;

        public bool HaveFreeSlot => _collected.Any(collected => collected.isActiveAndEnabled == false);

        public void CreateClip(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            
            if (_collected.Count >= capacity)
                return;

            Reset();
            
            for (int i = 0; i < capacity; i++)
            {
                Vector3 bagPosition = _container.position;
                Vector3 newPosition = new Vector3(bagPosition.x, bagPosition.y + i * _offsetY, bagPosition.z);
            
                CreateSlot(newPosition);
            }
            
            for (int i = 0; i < _activateCollected; i++)
                if (_collected[i].Type == Types.Bullet)
                    _collected[i].SetBullet();
                else
                    _collected[i].SetKey();
            
            RefreshLabelCapactity();
        }

        public void AddSlot(int currentCapacity)
        {
            if (_collected.Count >= currentCapacity)
                return;
            
            Vector3 bagPosition = _container.position;
            Vector3 newPosition = new Vector3(
                bagPosition.x, bagPosition.y + _collected.Count * _offsetY, bagPosition.z);
            
            CreateSlot(newPosition);
            RefreshLabelCapactity();
            Sort();
        }

        public void Reset()
        {
            foreach (Collected collected in _collected)
                collected.Inactive();
            
            _activateCollected = 0;
            RefreshLabelCapactity();
        }

        public void CollectBullet()
        {
            _activateCollected++;
            RefreshLabelCapactity();
            Sort();
        }

        public void UseCollectedBullet()
        {
            _activateCollected--;
            RefreshLabelCapactity();
            Sort();
        }

        public void CollecteKey()
        {
            if (_activateCollected >= _collected.Count)
                return;
            
            if (HaveKey())
                return;
            
            _activateCollected++;
            RefreshLabelCapactity();
            
            Collected key = _collected[_activateCollected - 1];
            key.SetKey();
            SetKeyPosition(key);
            
            Sort();
        }

        public void UseKey()
        {
            _activateCollected--;
            RefreshLabelCapactity();

            foreach (Collected collected in _collected)
                if (collected.Type == Types.Key)
                    collected.Inactive();
        }

        public bool HaveKey() => 
            _collected.Any(collected => collected.Type == Types.Key && collected.isActiveAndEnabled);

        private void CreateSlot(Vector3 newPosition)
        {
            Collected collected = Instantiate(_collectedPrefab, _container);
            collected.transform.position = newPosition;
            collected.Inactive();

            _collected.Add(collected);
        }

        private void RefreshLabelCapactity()
        {
            if (_activateCollected >= _collected.Count)
                ShowMaxLabel();
            else
                ShowCurrentFillCapacity();
        }

        private void Sort()
        {
            if (HaveKey())
            {
                if (_activateCollected > 1)
                {
                    for (int i = 0; i < _activateCollected - 1; i++)
                        _collected[i].SetBullet();
                }
                
                Collected key = _collected[_activateCollected - 1];
                key.SetKey();
                SetKeyPosition(key);
            }
            else
            {
                for (int i = 0; i < _activateCollected; i++)
                    _collected[i].SetBullet();
            }
            
            for (int i = _activateCollected; i < _collected.Count; i++)
                _collected[i].Inactive();
        }

        private void SetKeyPosition(Collected key)
        {
            Vector3 bagKeyPosition = _container.position;
            Vector3 newKeyPosition = new Vector3(
                bagKeyPosition.x, bagKeyPosition.y + (_activateCollected - 1) * _offsetY, bagKeyPosition.z);
            
            key.transform.position = newKeyPosition;
        }

        private void ShowMaxLabel() =>
            _maxLabel.text = MaxLabelText;
        
        private void ShowCurrentFillCapacity() => 
            _maxLabel.text = $"{_activateCollected} / {_collected.Count}";
    }
}