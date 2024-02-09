using System;
using UnityEngine;

namespace Source.Scripts.Players.Health
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 100;
    
        private int _currentHealth;
        private int _minHealth = 0;

        public event Action PlayerDie;
        public event Action<int, int> PlayerHealthChanged;

        private void Awake() => 
            _currentHealth = _maxHealth;

        private void Start() => 
            PlayerHealthChanged?.Invoke(_currentHealth, _maxHealth);
        
        public void TakeDamage(int damage)
        {
            if (damage < 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            _currentHealth = Mathf.Clamp(_currentHealth -= damage, _minHealth, _maxHealth);
            PlayerHealthChanged?.Invoke(_currentHealth, _maxHealth);
            
            if (_currentHealth <= _minHealth)
                PlayerDie?.Invoke();
        }
    }
}