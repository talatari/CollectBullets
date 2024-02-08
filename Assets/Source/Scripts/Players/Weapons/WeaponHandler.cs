using System.Collections;
using UnityEngine;

namespace Source.Scripts.Players.Weapons
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private BulletForPistol _bulletPrefab;
        
        private Weapon _weapon = new (new Pistol());
        private Coroutine _shootingCoroutine;
        
        public int ClipCapacityBullets => _weapon.ClipCapacityBullets;
        public int CollectedBullets => _weapon.CollectedBullets;

        private void OnDisable() => 
            StopShooting();

        public void StartShooting()
        {
            StopShooting();

            _shootingCoroutine = StartCoroutine(Shooting());
        }
        
        public void CollectBullet() => 
            _weapon.CollectBullet();

        public void StopShooting()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);
        }

        private IEnumerator Shooting()
        {
            while (enabled)
            {
                if (_weapon.CollectedBullets > 0)
                {
                    _weapon.Shoot();
                    
                    Vector3 startPosition = new Vector3(
                        transform.position.x, transform.position.y, transform.position.z);
                    
                    Instantiate(_bulletPrefab, startPosition, Quaternion.identity);
                }

                yield return new WaitForSeconds(_weapon.ShootingDelay);
            }
        }
    }
}