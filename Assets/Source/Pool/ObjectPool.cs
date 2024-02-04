using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Pool
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        
        protected IEnumerable<T> Pool => _pool;
        
        private List<T> _pool = new();
        private T _result;
        
        protected T GetObject(T prefab)
        {
            _result = _pool.FirstOrDefault(item => item.gameObject.activeSelf);
            
            if (_result == null)
            {
                _result = Instantiate(prefab, _container);
                _result.gameObject.SetActive(false);
                _pool.Add(_result);
            }
            
            return _result;
        }

        protected T GetObject(List<T> prefabs) => 
            GetObject(prefabs[Random.Range(0, prefabs.Count)]);
    }
}