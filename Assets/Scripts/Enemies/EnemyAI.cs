using UnityEngine;
using UnityEngine.AI;
using EZCameraShake;

public class EnemyAI : MonoBehaviour
{
    [Header("Agent")]
    [SerializeField] private float health = 100f;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] LayerMask whatIsGround, whatIsPlayer;

    [Header("Healthbar")]
    [SerializeField] private HealthBar _healthbar;
    [SerializeField] private GameObject _deathParticles;

    [Header("Agent Attacking")]
    [SerializeField] private float spawnForce = 35f;
    [SerializeField] GameObject projectile;
    [SerializeField] float timeBetweenAttacks;
    private bool alreadyAttacked;
    private Transform player;

    [Header("States")]
    [SerializeField] float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("-- PLAYER --").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(transform.position);
    }

    private void Start()
    {
        _healthbar.UpdateHealthBar(health);
    }


    private void Update()
    {
        // Check for sight and attack
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Laser"))
        {
            TakeDamage(10);
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(player.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Projectile

            SoundManager.PlaySound(SoundType.LASER, 0.05f);

            Rigidbody rb = Instantiate(projectile,transform.position, transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * spawnForce, ForceMode.Impulse);
            Destroy(rb.gameObject, 5f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
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
        CameraShaker.Instance.ShakeOnce(3f, 3f, .1f, .5f);
        SoundManager.PlaySound(SoundType.DEATH, 0.2f);
        PlayerHealth.killCount++;
        Instantiate(_deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
