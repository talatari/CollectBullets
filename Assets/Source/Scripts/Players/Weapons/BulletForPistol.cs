using Source.Scripts.Enemies;
using UnityEngine;

namespace Source.Scripts.Players.Weapons
{
    public class BulletForPistol : MonoBehaviour
    {
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _speed = 0.5f;
        
        public int Damage => _damage;

        // private void Start() => 
        //     Destroy(gameObject, 3f);

        private void Update() => 
            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}