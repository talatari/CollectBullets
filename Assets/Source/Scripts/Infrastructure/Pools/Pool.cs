using System;
using System.Collections.Generic;
using Source.Scripts.Infrastructure.Factories.Interfaces;
using Source.Scripts.Infrastructure.Pools.Interfaces;

namespace Source.Scripts.Infrastructure.Pools
{
    public class Pool<T> : IPool<T> where T : IPoolable
    {
        private readonly IFactory<T> _factoryBullet;
        private readonly int _startBulletCount;

        private Queue<T> _pool = new();
        private List<T> _activeBullets = new();

        public Pool(IFactory<T> factoryBullet, int startBulletCount)
        {
            _factoryBullet = factoryBullet ?? throw new ArgumentNullException(nameof(factoryBullet));
            
            if (startBulletCount < 0) 
                throw new ArgumentOutOfRangeException(nameof(startBulletCount));
            
            _startBulletCount = startBulletCount;
        }

        public int StartItemCount => _startBulletCount;
        public int AllItemsCount => _activeBullets.Count + _pool.Count;
        public List<T> ActiveItems => _activeBullets;

        public void Init()
        {
            for (int i = 0; i < _startBulletCount; i++)
                Release(_factoryBullet.Create());
        }

        public T Get()
        {
            T bullet;
            
            if (_pool.Count > 0)
            {
                bullet = _pool.Dequeue();
                SetActive(bullet);
                
                return bullet;
            }
            
            bullet = _factoryBullet.Create();
            SetActive(bullet);

            return bullet;
        }

        public void Release(T bullet)
        {
            if (bullet == null) 
                throw new ArgumentNullException(nameof(bullet));

            bullet.Disable();
            _pool.Enqueue(bullet);
            _activeBullets.Remove(bullet);
        }

        public void SetActive(T bullet)
        {
            bullet.Enable();
            _activeBullets.Add(bullet);
            
            bullet.Init(this);
        }
    }
}