using System;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    public class Health : MonoBehaviour
    {
        private int _maxHealth;
        private int _currentHealth;
        private int _minHealth = 0;

        public event Action EnemyDie;
        public event Action<int, int> EnemyHealthChanged;

        public void TakeDamage(int damage)
        {
            if (damage < 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            _currentHealth = Mathf.Clamp(_currentHealth -= damage, _minHealth, _maxHealth);
            EnemyHealthChanged?.Invoke(_currentHealth, _maxHealth);
            
            if (_currentHealth <= _minHealth)
                EnemyDie?.Invoke();
        }

        public void SetHealth(int health)
        {
            if (health <= 0) 
                throw new ArgumentOutOfRangeException(nameof(health));

            _maxHealth = health;
            _currentHealth = _maxHealth;
            EnemyHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
    }
}