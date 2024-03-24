using System;
using Source.Codebase.Infrastructure.Factories.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Codebase.Infrastructure.Factories
{
    public class FactoryProjectile<T> : IFactory<T> where T : Object
    {
        private readonly T _prefab;
        private readonly Transform _parent;

        public FactoryProjectile(T prefab, Transform parent)
        {
            _prefab = prefab ? prefab : throw new ArgumentNullException(nameof(prefab));
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
        }

        public T Create() => 
            Object.Instantiate(_prefab, _parent);
    }
}