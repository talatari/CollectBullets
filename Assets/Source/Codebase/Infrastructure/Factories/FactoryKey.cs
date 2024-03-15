using System;
using Source.Codebase.Infrastructure.Factories.Interfaces;
using Source.Codebase.Infrastructure.Services;
using Source.Codebase.Keys;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Codebase.Infrastructure.Factories
{
    public class FactoryKey : IFactory<Key>
    {
        private readonly Key _keyPrefab;
        private readonly Transform _parent;
        private readonly TargetProvider _targetProvider;

        public FactoryKey(Key keyPrefab, Transform parent, TargetProvider targetProvider)
        {
            _keyPrefab = keyPrefab ? keyPrefab : throw new ArgumentNullException(nameof(keyPrefab));
            _parent = parent ? parent : throw new ArgumentNullException(nameof(parent));
            _targetProvider = targetProvider ?? throw new ArgumentNullException(nameof(targetProvider));
        }

        public Key Create()
        {
            Key key = Object.Instantiate(_keyPrefab, _parent);
            key.SetTarget(_targetProvider.Target);
            
            return key;
        }
    }
}