using System;
using System.Collections;
using Source.Codebase.Common;
using Source.Codebase.Infrastructure.Pools.Interfaces;
using UnityEngine;

namespace Source.Codebase.Players
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        
        private IPool<Bullet> _pool;
        private CooldownTimer _timerToBlink;
        private Coroutine _startBlink;
        private float _timeToBlink = 6f;
        private float _blinkTime = 0.5f;
        private int _countBlink = 8;

        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            if (pool == null) 
                throw new ArgumentNullException(nameof(pool));
            
            _pool = pool as IPool<Bullet>;
            
            if (_pool == null)
                throw new ArgumentException("Pool must be of type IPool<Bullet>");
        }
        
        public void Update()
        {
            if (_timerToBlink == null)
                return;
            
            _timerToBlink.Tick(Time.deltaTime);

            if (_timerToBlink.IsFinished && _startBlink == null)
                _startBlink = StartCoroutine(StartBlink());
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            _timerToBlink = new CooldownTimer(_timeToBlink);
            _timerToBlink.Run();
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            StopCoroutine(StartBlink());
            _startBlink = null;
        }

        public void OnReleaseToPool() => 
            _pool.Release(this);

        private IEnumerator StartBlink()
        {
            WaitForSeconds blinkTime = new WaitForSeconds(_blinkTime);
            
            for (int i = 0; i < _countBlink; i++)
            {
                _meshRenderer.enabled = !_meshRenderer.enabled;

                yield return blinkTime;
            }

            OnReleaseToPool();
        }
    }
}