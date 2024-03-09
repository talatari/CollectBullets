using System;
using System.Collections;
using Source.Codebase.Players.PlayerModels;
using UnityEngine;

namespace Source.Codebase.Behaviour
{
    public class Damageable : MonoBehaviour
    {
        private const float RegenerationDelay = 1f;
        
        private int _minHealth = 0;
        private int _maxHealth;
        private int _currentHealth;
        private int _regeneration;
        private HealthStats _healthStats;
        private Coroutine _coroutineRegeneration;
        private bool _isInit;

        public event Action Died;
        public event Action<int, int> HealthChanged;

        public void Init(int maxHealth)
        {
            if (maxHealth <= 0) 
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void Init(HealthStats healthStats)
        {
            _healthStats = healthStats ?? throw new ArgumentNullException(nameof(healthStats));
            
            Init(_healthStats.MaxHealth);
            _regeneration = _healthStats.Regeneration;
            
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

            StopRegeneration();
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

        public void Heal(int heal)
        {
            if (heal <= 0) 
                throw new ArgumentOutOfRangeException(nameof(heal));
            
            _currentHealth = Mathf.Clamp(_currentHealth += heal, _minHealth, _maxHealth);
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void FullRecovery()
        {
            _currentHealth = _maxHealth;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        private void OnSetMaxHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
            FullRecovery();
        }

        private void OnSetRegeneration(int regeneration)
        {
            _regeneration = regeneration;
            StartRegeneration();
        }

        private void StartRegeneration()
        {
            StopRegeneration();
            
            if (_regeneration > 0)
                _coroutineRegeneration = StartCoroutine(Regeneration());
        }

        private void StopRegeneration()
        {
            if (_coroutineRegeneration != null)
                StopCoroutine(_coroutineRegeneration);
        }

        private IEnumerator Regeneration()
        {
            WaitForSeconds regenerationDelay = new WaitForSeconds(RegenerationDelay);
            
            while (enabled)
            {
                Heal(_regeneration);
                
                yield return regenerationDelay;
            }
        }
    }
}