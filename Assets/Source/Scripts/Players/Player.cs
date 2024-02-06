using UnityEngine;

namespace Source.Scripts.Players
{
    [RequireComponent(typeof(CollisionHandler))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject _collectedBulletPrefab;
        [SerializeField] private Transform _bag;
        [SerializeField] private int _maxCapacityBullets = 5;
        
        private CollisionHandler _collisionHandler;
        private int _collectedBulletCount;
        private float _offsetY = 0.35f;

        public Transform Position { get; set; }
        public int CollectedBulletCount => _collectedBulletCount;
        public int MaxCapacityBullets => _maxCapacityBullets;

        private void Awake()
        {
            _collisionHandler = GetComponent<CollisionHandler>();
            _collisionHandler.Init(this);
        }

        private void OnEnable() => 
            _collisionHandler.BulletCollected += OnBulletCollected;

        private void Update() => 
            Position = transform;

        private void OnDisable() => 
            _collisionHandler.BulletCollected -= OnBulletCollected;

        private void OnBulletCollected()
        {
            Vector3 newPosition = new Vector3(
                _bag.transform.position.x,
                _bag.transform.position.y + _collectedBulletCount * _offsetY,
                _bag.transform.position.z);
            
            _collectedBulletCount++;

            GameObject collectedBullet = Instantiate(_collectedBulletPrefab, _bag);
            collectedBullet.transform.position = newPosition;
        }
    }
}