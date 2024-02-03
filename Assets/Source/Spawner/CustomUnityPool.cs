using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Spawner
{
    public class CustomUnityPool<T> where T : MonoBehaviour
    {
        private T _prefab;
        private List<T> _objects;

        public CustomUnityPool(T prefab, int prewarmObjects)
        {
            _prefab = prefab;
            _objects = new List<T>();

            for (int i = 0; i < prewarmObjects; i++)
            {
                T obj = Object.Instantiate(_prefab);
                obj.gameObject.SetActive(false);
                _objects.Add(obj);
            }
        }

        public T Get()
        {
            T obj = _objects.FirstOrDefault(x => !x.isActiveAndEnabled);

            if (obj == null)
                obj = Create();

            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Release(T obj) => 
            obj.gameObject.SetActive(false);

        private T Create()
        {
            T obj = Object.Instantiate(_prefab);
            _objects.Add(obj);
            return obj;
        }
    }
}