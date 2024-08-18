using EZCameraShake;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ElementHealth : MonoBehaviour
{
    public static bool breakDialogue = false;

    [SerializeField] private GameObject healthbar;
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private int _maxHealth = 250;
    [SerializeField] private GameObject deadFX;
    [Header("Dialogue")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private string[] lines;


    private int health;
    private bool damageApplied;

    private Camera _cam;

    private void Awake()
    {
        health = _maxHealth;
        _cam = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser") && damageApplied == false)
        {
            health -= 10;
            UpdateHealthBar(health);
            damageApplied = true;

            StartCoroutine(DamageCooldown());
        }

        if (health <= 0)
        {
            // dialogue
            if (breakDialogue == false)
            {
                dialogue.StartDialogue(lines);
                // show dialogue
                breakDialogue = true;
            }

            SoundManager.PlaySound(SoundType.DEATH, 0.4f);
            CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 2f);
            Instantiate(deadFX, transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(0.05f);
        damageApplied = false;
    }

    public void UpdateHealthBar(float currentHealth)
    {
        _healthbarSprite.fillAmount = currentHealth / _maxHealth;
    }

    public void Update()
    {
        healthbar.transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
