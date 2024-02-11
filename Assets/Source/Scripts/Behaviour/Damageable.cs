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

        public void SetMaxHealth(int maxHealth)
        {
            if (maxHealth <= 0) 
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));

            float tempCurrHP = _currentHealth;
            
            _currentHealth = Mathf.Clamp(_currentHealth -= damage, _minHealth, _maxHealth);
            HealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth == 0)
            {
                print($"tempCurrHP: {tempCurrHP}");
                print($"_currentHealth: {_currentHealth}");
            }
            
            if (_currentHealth <= _minHealth)
                Died?.Invoke();
        }
    }
}