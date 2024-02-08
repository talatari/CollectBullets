using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Players
{
    public class Bag : MonoBehaviour
    {
        [SerializeField] private GameObject _collectedBulletPrefab;
        
        private float _offsetY = 0.35f;
        private List<GameObject> _collectedBullets = new ();
        
        public void CollectBullet(int collectedBullets)
        {
            if (_collectedBullets.Count == 0)
            {
                Vector3 bagPosition = transform.position;
                Vector3 newPosition = new Vector3(
                    bagPosition.x, bagPosition.y + collectedBullets * _offsetY, bagPosition.z);
            
                GameObject collectedBullet = Instantiate(_collectedBulletPrefab, transform);
                collectedBullet.transform.position = newPosition;
                _collectedBullets.Add(collectedBullet);
            }
            else
            {
                // todo: fix index out of range
                _collectedBullets[collectedBullets].SetActive(true);
            }
        }

        public void UseCollectedBullets() => 
            _collectedBullets[_collectedBullets.Count - 1].SetActive(false);
    }
}