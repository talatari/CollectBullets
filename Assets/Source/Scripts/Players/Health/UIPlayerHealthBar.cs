using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Players.Health
{
    public class UIPlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private Image _fillBar;
        [SerializeField] private PlayerHealth _layerPlayerHealth;
        
        private void OnEnable() => 
            _layerPlayerHealth.PlayerHealthChanged += OnRefreshHealthBar;

        private void OnDisable() => 
            _layerPlayerHealth.PlayerHealthChanged -= OnRefreshHealthBar;

        private void OnRefreshHealthBar(int currentHealth, int maxHealth)
        {
            _tmpText.text = currentHealth + " / " + maxHealth;
            _fillBar.fillAmount = (float) currentHealth / maxHealth;
        }
    }
}