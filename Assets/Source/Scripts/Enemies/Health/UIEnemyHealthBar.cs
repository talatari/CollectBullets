using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Enemies.Health
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private Image _fillBar;
        [SerializeField] private EnemyHealth _enemyEnemyHealth;
        
        private void OnEnable() => 
            _enemyEnemyHealth.EnemyHealthChanged += OnRefreshHealthBar;

        private void OnDisable() => 
            _enemyEnemyHealth.EnemyHealthChanged -= OnRefreshHealthBar;

        private void OnRefreshHealthBar(int currentHealth, int maxHealth)
        {
            _tmpText.text = currentHealth + " / " + maxHealth;
            _fillBar.fillAmount = (float) currentHealth / maxHealth;
        }
    }
}