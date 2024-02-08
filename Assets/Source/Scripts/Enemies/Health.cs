using System;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 20;
    
        private int _currentHealth;
        private int _minHealth = 0;

        public event Action EnemyDie;
        public event Action<int, int> HealthChanged;

        private void Awake() => 
            _currentHealth = _maxHealth;

        private void Start() => 
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        
        public void TakeDamage(int damage)
        {
            if (damage < 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            _currentHealth = Mathf.Clamp(_currentHealth -= damage, _minHealth, _maxHealth);
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
            
            print($"_currentHealth: {_currentHealth}");
            
            if (_currentHealth <= _minHealth)
                EnemyDie?.Invoke();
        }
    }
}