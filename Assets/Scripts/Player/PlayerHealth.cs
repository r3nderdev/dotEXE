using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using EZCameraShake;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public static int killCount;
    [SerializeField] private float health;

    [Header("Damage Values")]
    [SerializeField] private float laserDamage;
    [SerializeField] private float sniperLaserDamage;

    private bool damageApplied = false;
    private float damage;

    [Header("Damage Vignette")]
    [SerializeField] private PostProcessVolume _volume;
    private Vignette _vignette;

    [Header("GreenFX")]
    [SerializeField] private GameObject greenFX;

    [Header("Level Loader")]
    [SerializeField] private LevelLoader levelLoader;

    void Start()
    {
        _volume.profile.TryGetSettings<Vignette>(out _vignette);

        if (!_vignette)
        {
            Debug.Log("Error, Vignette Empty");
        }
        else
        {
            _vignette.enabled.Override(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {
           damage = laserDamage;
           SoundManager.PlaySound(SoundType.HIT, 0.2f);
           if (damageApplied == false) UpdateHealth();

            damageApplied = true;
        }

        else if (other.CompareTag("Sniper Laser"))
        {
            damage = sniperLaserDamage;

            SoundManager.PlaySound(SoundType.HIT, 0.2f);
            if (damageApplied == false) UpdateHealth();

            damageApplied = true;
        }

        else if (other.CompareTag("Health"))
        {
            if (damageApplied == false)
            {
                health += 10f;
                health = Mathf.Clamp(health, 0, 100);
                //Debug.Log("Health: " + health);

                UpdateVignette();

                Instantiate(greenFX, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);

                damageApplied = true;

                StartCoroutine(DamageCheck());
            }
        }
    }

    public void DealDamage(int dealtDamage)
    {
        damage = dealtDamage;

        SoundManager.PlaySound(SoundType.HIT, 0.3f);
        if (damageApplied == false) UpdateHealth();

        damageApplied = true;
    }

    private void UpdateHealth()
    {
        CameraShaker.Instance.ShakeOnce(2.4f, 2f, .1f, .5f);

        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        //Debug.Log("Health: " + health);

        UpdateVignette();

        // Player Death
        if (health <= 0) Invoke(nameof(DestroyPlayer), .05f);

        StartCoroutine(DamageCheck());
    }

    private void UpdateVignette()
    {
        // Vignette
        _vignette.enabled.Override(true);
        _vignette.intensity.Override((1 - health / 100) - 0.15f);
        _vignette.enabled.Override(true);
    }

    private IEnumerator DamageCheck()
    {
        yield return new WaitForSeconds(0.05f);

        damageApplied = false;
    }

    private void DestroyPlayer()
    {
        levelLoader.LoadNextLevel(0f, 0);
    }
}
