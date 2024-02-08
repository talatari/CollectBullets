using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Players
{
    public class Bag : MonoBehaviour
    {
        [SerializeField] private GameObject _collectedBulletPrefab;
        
        private float _offsetY = 0.35f;
        private List<GameObject> _collectedBullets = new ();
        
        public void CreateClip(int capacity)
        {
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
        }

        public void CollectBullet(int collectedBullets) => 
            _collectedBullets[collectedBullets - 1].SetActive(true);

        public void UseCollectedBullets(int collectedBullets) => 
            _collectedBullets[collectedBullets].SetActive(false);
    }
}