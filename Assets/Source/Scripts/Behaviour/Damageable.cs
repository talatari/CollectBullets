using System;
using System.Collections;
using Source.Scripts.Players.PlayerModels;
using UnityEngine;

namespace Source.Scripts.Behaviour
{
    public class Damageable : MonoBehaviour
    {
        private const float RatioIncrement = 0.5f;
        
        private int _minHealth = 0;
        private int _maxHealth;
        private int _currentHealth;
        private float _regeneration;
        private int _baseRegeneration;
        private HealthStats _healthStats;
        private bool _isInit;

        public event Action Died;
        public event Action<int, int> HealthChanged;

        public void Init(int maxHealth)
        {
            if (maxHealth <= 0 || maxHealth <= _currentHealth) 
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void Init(HealthStats healthStats)
        {
            _healthStats = healthStats ?? throw new ArgumentNullException(nameof(healthStats));
            
            Init(_healthStats.MaxHealth);
            
            _baseRegeneration = _healthStats.Regeneration;
            _regeneration = _baseRegeneration;

            _isInit = true;
            
            _healthStats.MaxHealthChanged += OnSetMaxHealth;
            _healthStats.RegenerationChanged += OnSetRegeneration;
        }

        private void OnDisable()
        {
            if (_isInit == false)
                return;
            
            _healthStats.MaxHealthChanged -= OnSetMaxHealth;
            _healthStats.RegenerationChanged -= OnSetRegeneration;
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

        public void FullRecovery()
        {
            _currentHealth = _maxHealth;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void Heal(int heal)
        {
            if (heal <= 0) 
                throw new ArgumentOutOfRangeException(nameof(heal));
            
            _currentHealth = Mathf.Clamp(_currentHealth += heal, _minHealth, _maxHealth);
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        private void OnSetMaxHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        private void OnSetRegeneration(int regeneration)
        {
            float newRegeneration = _baseRegeneration + (regeneration - _baseRegeneration) * RatioIncrement;
            
            _regeneration = newRegeneration;
        }

        private IEnumerator Regeneration()
        {
            // TODO: implement
            return null;
        }
    }
}