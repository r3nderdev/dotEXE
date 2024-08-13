using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private float _maxHealth = 100f;

    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    public void UpdateHealthBar(float currentHealth)
    {
        _healthbarSprite.fillAmount = currentHealth / _maxHealth;
    }

    public void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
