using Source.Scripts.Infrastructure.Pools.Interfaces;
using UnityEngine;

namespace Source.Scripts.Keys
{
    public class Key : MonoBehaviour, IPoolable
    {
        public void Init<T>(IPool<T> pool) where T : IPoolable
        {
            throw new System.NotImplementedException();
        }

        public void Enable()
        {
            throw new System.NotImplementedException();
        }

        public void Disable()
        {
            throw new System.NotImplementedException();
        }

        public void ReleaseToPool()
        {
            throw new System.NotImplementedException();
        }
    }
}