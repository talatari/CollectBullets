using System;
using Source.Scripts.Infrastructure.Bullets;
using UnityEngine;

namespace Source.Scripts.Bullets
{
    public class Bullet : MonoBehaviour
    {
        private PoolBullet _poolBullet;

        public void Init(PoolBullet poolBullet)
        {
            if (poolBullet == null) 
                throw new ArgumentNullException(nameof(poolBullet));

            _poolBullet = poolBullet;
        }

        public void ReleaseToPool() => 
            _poolBullet.Release(this);
    }
}