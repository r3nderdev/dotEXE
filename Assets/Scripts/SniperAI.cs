using System.Collections;
using UnityEngine;
public class SniperAI : MonoBehaviour
{
    [Header("Stuff")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float waitTime = 3f;
    [SerializeField] private float blinkTime = 0.2f;
    [SerializeField] LayerMask whatIsPlayer;

    [Header("Laser")]
    [SerializeField] private float shootForce;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform shootPoint;

    [Header("Healthbar")]
    [SerializeField] private HealthBar _healthbar;
    [SerializeField] private GameObject _deathParticles;

    [Header("Attacking")]
    [SerializeField] GameObject projectile;
    [SerializeField] float timeBetweenAttacks;
    private bool alreadyAttacked;
    private Transform player;

    [Header("States")]
    [SerializeField] float attackRange;
    private bool playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("-- PLAYER --").transform;
        lineRenderer.positionCount = 2;
    }

    private void Start()
    {
        _healthbar.UpdateHealthBar(health);
    }


    private void Update()
    {
        // Check for attack
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange)
        {
            lineRenderer.SetPosition(0, shootPoint.transform.position);
            lineRenderer.SetPosition(1, player.position);
            AttackPlayer();
        }

        if (!playerInAttackRange)
        {
            lineRenderer.gameObject.SetActive(false);
            StopAllCoroutines();
            ResetAttack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Laser"))
        {
            TakeDamage(10);
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

        yield return new WaitForSeconds(blinkTime);

        lineRenderer.gameObject.SetActive(false);

        yield return new WaitForSeconds(blinkTime);

        lineRenderer.gameObject.SetActive(true);

        yield return new WaitForSeconds(blinkTime-0.1f);



        // Projectile

        SoundManager.PlaySound(SoundType.LASER, 0.05f);

        Rigidbody rb = Instantiate(projectile, shootPoint.position, shootPoint.rotation).GetComponent<Rigidbody>();
        rb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
        Destroy(rb.gameObject, 3f);

        lineRenderer.gameObject.SetActive(false);

        Invoke(nameof(ResetAttack), timeBetweenAttacks);

        yield return null;
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
        SoundManager.PlaySound(SoundType.DEATH, 0.2f);
        Instantiate(_deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
