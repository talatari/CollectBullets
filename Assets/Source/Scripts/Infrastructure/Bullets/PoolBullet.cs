using System;
using System.Collections.Generic;
using Source.Scripts.Bullets;

namespace Source.Scripts.Infrastructure.Bullets
{
    public class PoolBullet
    {
        private readonly FactoryBullet _factoryBullet;
        private readonly int _startBulletCount;

        private Queue<Bullet> _pool = new();
        private List<Bullet> _activeBullets = new();

        public PoolBullet(FactoryBullet factoryBullet, int startBulletCount)
        {
            _factoryBullet = factoryBullet ?? throw new ArgumentNullException(nameof(factoryBullet));
            
            if (startBulletCount < 0) 
                throw new ArgumentOutOfRangeException(nameof(startBulletCount));
            
            _startBulletCount = startBulletCount;
        }

        public int StartBulletCount => _startBulletCount;
        public int GetAmountAllBullets => _activeBullets.Count + _pool.Count;
        public List<Bullet> ActiveBullets => _activeBullets;

        public void Init()
        {
            for (int i = 0; i < _startBulletCount; i++)
                Release(_factoryBullet.Create());
        }

        public Bullet Get()
        {
            Bullet bullet;
            
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

        public void Release(Bullet bullet)
        {
            if (bullet == null) 
                throw new ArgumentNullException(nameof(bullet));
            
            bullet.gameObject.SetActive(false);
            _pool.Enqueue(bullet);
            _activeBullets.Remove(bullet);
        }

        public void SetActive(Bullet bullet)
        {
            bullet.gameObject.SetActive(true);
            _activeBullets.Add(bullet);
        }
    }
}