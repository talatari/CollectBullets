using Source.Enemies;
using Source.Pool;
using UnityEngine;

namespace Source.Spawners
{
    public abstract class ProjectileSpawner : ObjectPool<Projectile>
    {
        [SerializeField] private Projectile _projectilePrefab;
        // [SerializeField] private bool _isBulletsPlayer;
    
        public void Shoot(Transform shootPoint, Vector3 direction)
        {
            Projectile projectile = GetObject(_projectilePrefab);
            projectile.transform.position = shootPoint.position;
            projectile.gameObject.SetActive(true);
        }
    }
}