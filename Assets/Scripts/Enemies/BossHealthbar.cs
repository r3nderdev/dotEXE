using UnityEngine;
using UnityEngine.UI;

public class BossHealthbar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private float _maxHealth = 100f;

    public void UpdateHealthBar(float currentHealth)
    {
        _healthbarSprite.fillAmount = currentHealth / _maxHealth;
    }

}
