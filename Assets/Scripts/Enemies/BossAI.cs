using EZCameraShake;
using System.Collections;
using UnityEngine;
public class BossAI : MonoBehaviour
{
    [Header("Stuff")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float waitTime = 3f;
    [SerializeField] private float blinkTime = 0.2f;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] private LevelLoader levelLoader;

    [Header("Laser")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask rayCastMask;

    [Header("Healthbar")]
    [SerializeField] private BossHealthbar _healthbar;
    [SerializeField] private GameObject healthbarObject;
    [SerializeField] private GameObject deathFX;

    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;
    private Transform player;
    [SerializeField] private GameObject BossImpactFX;

    [Header("Laser Pulse")]
    [SerializeField] private GameObject core;
    [SerializeField] private float pulseScale = 1.2f;
    [SerializeField] private float pulseDuration = 0.2f;
    [SerializeField] private float pulseUpDuration = 0.1f;
    private Vector3 originalScale;


    [Header("States")]
    [SerializeField] private float attackRange;
    private bool playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("-- PLAYER --").transform;
        lineRenderer.positionCount = 2;
        originalScale = core.transform.localScale;
    }

    private void Start()
    {
        _healthbar.UpdateHealthBar(health);
    }


    private void Update()
    {
        transform.LookAt(player);

        // Check for attack
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange)
        {
            if (healthbarObject.activeSelf == false) healthbarObject.SetActive(true);

            Vector3 directionToPlayer = (player.position - shootPoint.position).normalized;

            if (Physics.Raycast(shootPoint.position, directionToPlayer, out RaycastHit hit, Mathf.Infinity, rayCastMask))
            {
                lineRenderer.SetPosition(0, shootPoint.transform.position);
                lineRenderer.SetPosition(1, hit.point);
                AttackPlayer();
            }
        }

        if (!playerInAttackRange)
        {
            lineRenderer.gameObject.SetActive(false);
            StopAllCoroutines();
            ResetAttack();
        }
    }


    private void AttackPlayer()
    {
        shootPoint.transform.LookAt(player);

        if (!alreadyAttacked)
        {
            StartCoroutine(ShootLaser());
        }
    }

    IEnumerator ShootLaser()
    {
        alreadyAttacked = true;

        // Warning laser

        lineRenderer.gameObject.SetActive(true);

        yield return new WaitForSeconds(waitTime);
        lineRenderer.gameObject.SetActive(false);

        yield return new WaitForSeconds(blinkTime);
        lineRenderer.gameObject.SetActive(true);

        yield return new WaitForSeconds(blinkTime - 0.1f);
        lineRenderer.gameObject.SetActive(false);

        yield return new WaitForSeconds(blinkTime - 0.15f);
        lineRenderer.gameObject.SetActive(true);

        yield return new WaitForSeconds(blinkTime - 0.2f);
        lineRenderer.gameObject.SetActive(false);

        yield return new WaitForSeconds(blinkTime - 0.25f);
        lineRenderer.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);



        // Shoot laser

        SoundManager.PlaySound(SoundType.BOSS_LASER, 0.5f);

        Vector3 directionToPlayer = (player.position - shootPoint.position).normalized;

        StartCoroutine(PulseEffect());

        if (Physics.Raycast(shootPoint.position, directionToPlayer, out RaycastHit hit, Mathf.Infinity, rayCastMask))
        {

            CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1.2f);
            Instantiate(BossImpactFX,hit.point,Quaternion.identity);

            if (hit.transform == player)
            {
                // Deal damage to player
                playerHealth.DealDamage(50);
            }
        }

        lineRenderer.gameObject.SetActive(false);

        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private IEnumerator PulseEffect()
    {

        float elapsedTime = 0f;

        // Scale up
        while (elapsedTime < pulseDuration)
        {
            float t = elapsedTime / pulseUpDuration;
            core.transform.localScale = Vector3.Lerp(originalScale, originalScale * pulseScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        core.transform.localScale = originalScale * pulseScale;

        elapsedTime = 0f;

        // Scale back down
        while (elapsedTime < pulseDuration)
        {
            float t = elapsedTime / pulseDuration;
            core.transform.localScale = Vector3.Lerp(originalScale * pulseScale, originalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset
        core.transform.localScale = originalScale;
    }

private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        _healthbar.UpdateHealthBar(health);

        if (health <= 0) Invoke(nameof(DestroyEnemy), .05f);
    }

    private void DestroyEnemy()
    {
        CameraShaker.Instance.ShakeOnce(5f, 5f, .1f, 1.2f);

        SoundManager.PlaySound(SoundType.BOSS_EXPLOSION, 1f);

        Instantiate(deathFX, transform.position, Quaternion.identity);

        healthbarObject.SetActive(false);
        Destroy(gameObject);

        levelLoader.LoadNextLevel(2, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
