using System;
using UnityEngine;

namespace Source.Scripts.Behaviour
{
    public class Damageable : MonoBehaviour
    {
        private int _minHealth = 0;
        private int _maxHealth;
        private int _currentHealth;

        public event Action Died;
        public event Action<int, int> HealthChanged;

        private void Start()
        {
            _currentHealth = _maxHealth;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void SetMaxHealth(int maxHealth)
        {
            if (maxHealth <= 0) 
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            
            _maxHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            _currentHealth = Mathf.Clamp(_currentHealth -= damage, _minHealth, _maxHealth);
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
            
            if (_currentHealth <= _minHealth)
                Died?.Invoke();
        }
    }
}