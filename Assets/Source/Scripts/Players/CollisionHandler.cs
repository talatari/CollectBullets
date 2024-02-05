using Source.Scripts.Bullets;
using UnityEngine;

namespace Source.Scripts.Players
{
    public class CollisionHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            print($"Player OnTriggerEnter with {other.name}");
            
            if (other.TryGetComponent(out Bullet bullet)) 
                bullet.ReleaseToPool();
        }

        private void OnCollisionEnter(Collision other)
        {
            print($"Player OnCollisionEnter with {other.gameObject.name}");
            
            if (other.gameObject.TryGetComponent(out Bullet bullet))
                bullet.ReleaseToPool();
        }
    }
}