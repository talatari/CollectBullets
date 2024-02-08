using Source.Scripts.Players.CollisionHandlers;
using Source.Scripts.Players.Movement;
using Source.Scripts.Players.Movement.Joystick;
using UnityEngine;

namespace Source.Scripts.Players
{
    [RequireComponent(typeof(CollisionForBullets), typeof(Mover))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject _collectedBulletPrefab;
        [SerializeField] private Transform _bag;
        [SerializeField] private int _maxCapacityBullets = 5;
        [SerializeField] private JoystickForRotator _joystickForRotator;
        
        private CollisionForBullets _collisionForBullets;
        private CollisionForEnemies _collisionForEnemies;
        private int _collectedBulletCount;
        private float _offsetY = 0.35f;

        public Transform Position { get; set; }
        public int CollectedBulletCount => _collectedBulletCount;
        public int MaxCapacityBullets => _maxCapacityBullets;

        private void Awake()
        {
            _collisionForBullets = GetComponent<CollisionForBullets>();
            _collisionForBullets.Init(this);
            _collisionForEnemies = GetComponentInChildren<CollisionForEnemies>();
            _collisionForEnemies.Init(this);
        }

        private void OnEnable() => 
            _collisionForBullets.BulletCollected += OnCollected;

        private void Update() => 
            Position = transform;

        private void OnDisable() => 
            _collisionForBullets.BulletCollected -= OnCollected;

        public void RotateToEnemy(Vector3 direction) => 
            _joystickForRotator.SetEnemyPosition(direction);

        private void OnCollected()
        {
            Vector3 bagPosition = _bag.transform.position;
            Vector3 newPosition = new Vector3(
                bagPosition.x, bagPosition.y + _collectedBulletCount * _offsetY, bagPosition.z);
            
            _collectedBulletCount++;

            GameObject collectedBullet = Instantiate(_collectedBulletPrefab, _bag);
            collectedBullet.transform.position = newPosition;
        }
    }
}