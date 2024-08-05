using System.Collections;
using UnityEngine;
using EZCameraShake;

public class BossTarget : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private int playerLaserDamage = 10;
    [SerializeField] private GameObject element;
    [SerializeField] private BossAI bossScript;

    [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    private Renderer rend;

    [Header("Death Effects")]
    [SerializeField] private Transform coreTransform;
    [SerializeField] private GameObject deadFX;

    private int health = 200;
    private bool canTakeDamage = false;


    private void Start()
    {
        rend = GetComponent<Renderer>();
        StartCoroutine(WaitForElement());
    }

    IEnumerator WaitForElement()
    {
        yield return new WaitUntil(() => element == null);
        rend.material = defaultMaterial;
        canTakeDamage = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser") && canTakeDamage == true)
        {
            bossScript.TakeDamage(playerLaserDamage);
            health -= playerLaserDamage;
        }

        if (health <= 0)
        {
            //CameraShaker.Instance.ShakeOnce(3f, 4f, .1f, 1f);
            SoundManager.PlaySound(SoundType.BOSS_EXPLOSION, 0.7f);
            // Spawn some sick particles
            Instantiate(deadFX, coreTransform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
