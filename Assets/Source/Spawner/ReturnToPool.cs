using System;
using Source.Enemies;
using UnityEngine;
using UnityEngine.Pool;

namespace Source.Spawner
{
    public class ReturnToPool : MonoBehaviour
    {
        private IObjectPool<Enemy> _pool;
        private Enemy _enemy;
        
        public void Init(IObjectPool<Enemy> pool, Enemy enemy)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _enemy = enemy ? enemy : throw new ArgumentNullException(nameof(enemy));

            _pool = pool;
        }

        public void Release() => 
            _pool.Release(_enemy);
    }
}