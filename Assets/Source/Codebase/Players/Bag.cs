using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Codebase.Players
{
    public class Bag : MonoBehaviour
    {
        [SerializeField] private GameObject _collectedBulletPrefab;
        [SerializeField] private GameObject _collectedKeyPrefab;
        
        private float _offsetY = 0.35f;
        private List<GameObject> _collectedBullets;
        private int _activatedBulletsCount;
        private Transform _parent;
        private GameObject _collectedKey;
       
        public void CreateClip(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            ClearBag();
            
            _collectedBullets = new List<GameObject>();
            _parent = transform;
            
            for (int i = 0; i < capacity; i++)
            {
                Vector3 bagPosition = _parent.position;
                Vector3 newPosition = new Vector3(bagPosition.x, bagPosition.y + i * _offsetY, bagPosition.z);
            
                GameObject collectedBullet = Instantiate(_collectedBulletPrefab, _parent);
                collectedBullet.transform.position = newPosition;
                collectedBullet.SetActive(false);

                _collectedBullets.Add(collectedBullet);
            }
            
            Vector3 bagKeyPosition = _parent.position;
            Vector3 newKeyPosition = new Vector3(
                bagKeyPosition.x, bagKeyPosition.y + _collectedBullets.Count * _offsetY, bagKeyPosition.z);
            
            _collectedKey = Instantiate(_collectedKeyPrefab, _parent);
            _collectedKey.transform.position = newKeyPosition;
            _collectedKey.SetActive(false);
            
            _collectedBullets.Add(_collectedKey);

            for (int i = 0; i < _activatedBulletsCount; i++)
                _collectedBullets[i].SetActive(true);
        }

        public void Reset()
        {
            _activatedBulletsCount = 0;
            
            foreach (GameObject collectedBullet in _collectedBullets)
                collectedBullet.SetActive(false);
        }

        private void ClearBag()
        {
            if (_collectedBullets != null)
                foreach (GameObject collectedBullet in _collectedBullets)
                    Destroy(collectedBullet);
        }

        public void CollectBullet(int collectedBullets)
        {
            if (collectedBullets <= 0) 
                throw new ArgumentOutOfRangeException(nameof(collectedBullets));
            
            _collectedBullets[collectedBullets - 1].SetActive(true);
            _activatedBulletsCount++;
            
            if (_collectedKey.activeSelf)
                SetKeyPosition();
            
            if (_activatedBulletsCount >= _collectedBullets.Count)
                ShowMaxLabel();
        }

        public void UseCollectedBullets(int collectedBullets)
        {
            if (collectedBullets < 0) 
                throw new ArgumentOutOfRangeException(nameof(collectedBullets));
            
            _collectedBullets[collectedBullets].SetActive(false);
            _activatedBulletsCount--;
            
            if (_collectedKey.activeSelf)
                SetKeyPosition();

            if (_activatedBulletsCount < _collectedBullets.Count)
                HideMaxLabel();
        }

        public void CollecteKey()
        {
            SetKeyPosition();

            _collectedKey.SetActive(true);
        }

        public void UseKey() => 
            _collectedKey.SetActive(false);

        private void SetKeyPosition()
        {
            Vector3 bagKeyPosition = _parent.position;
            Vector3 newKeyPosition = new Vector3(
                bagKeyPosition.x, bagKeyPosition.y + _activatedBulletsCount * _offsetY, bagKeyPosition.z);

            _collectedKey.transform.position = newKeyPosition;
        }

        private void ShowMaxLabel()
        {
            // TODO: включаем отображение надписи MAX, когда все слоты заняты
        }

        private void HideMaxLabel()
        {
            // TODO: отключаем отображение надписи MAX, когда не все слоты заняты
        }
    }
}