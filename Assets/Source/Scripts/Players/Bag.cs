using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Players
{
    public class Bag : MonoBehaviour
    {
        [SerializeField] private GameObject _collectedBulletPrefab;
        
        private float _offsetY = 0.35f;
        private List<GameObject> _collectedBullets;
        private int _activatedBulletsCount;
       
        public void CreateClip(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            ClearBag();
            
            _collectedBullets = new List<GameObject>();
            
            for (int i = 0; i < capacity; i++)
            {
                Vector3 bagPosition = transform.position;
                Vector3 newPosition = new Vector3(
                    bagPosition.x, bagPosition.y + i * _offsetY, bagPosition.z);
            
                GameObject collectedBullet = Instantiate(_collectedBulletPrefab, transform);
                collectedBullet.transform.position = newPosition;
                collectedBullet.SetActive(false);

                _collectedBullets.Add(collectedBullet);
            }

            for (int i = 0; i < _activatedBulletsCount; i++)
                _collectedBullets[i].SetActive(true);
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
            
            if (_activatedBulletsCount >= _collectedBullets.Count)
                ShowMaxLabel();
        }

        public void UseCollectedBullets(int collectedBullets)
        {
            if (collectedBullets < 0) 
                throw new ArgumentOutOfRangeException(nameof(collectedBullets));
            
            _collectedBullets[collectedBullets].SetActive(false);
            _activatedBulletsCount--;

            if (_activatedBulletsCount < _collectedBullets.Count)
                HideMaxLabel();
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