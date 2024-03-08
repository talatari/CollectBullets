using Source.Codebase.Behaviour;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Codebase.UI
{
    public class UIHealthBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private Image _fillBar;
        [SerializeField] private Damageable _health;

        private void OnEnable() => 
            _health.HealthChanged += OnRefreshHealthBar;

        private void OnDisable() => 
            _health.HealthChanged -= OnRefreshHealthBar;

        private void OnRefreshHealthBar(int currentHealth, int maxHealth)
        {
            _tmpText.text = currentHealth + " / " + maxHealth;
            _fillBar.fillAmount = (float) currentHealth / maxHealth;
        }
    }
}