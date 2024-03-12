using System;
using Source.Codebase.Infrastructure.Factories.Interfaces;
using Source.Codebase.Keys;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Codebase.Infrastructure.Factories
{
    public class FactoryKey : IFactory<Key>
    {
        private readonly Transform _parent;
        private readonly Key _keyPrefab;
        
        public FactoryKey(Key keyPrefab, Transform parent)
        {
            _keyPrefab = keyPrefab ? keyPrefab : throw new ArgumentNullException(nameof(keyPrefab));
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
        }

        public Key Create() => 
            Object.Instantiate(_keyPrefab, _parent);
    }
}