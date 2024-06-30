using UnityEngine;
using UnityEngine.AI;
using InfimaGames.LowPolyShooterPack;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public Movement movement;

    public Transform player;
    BoxCollider boxCollider;
    public LayerMask whatIsGround, whatIsPlayer;

    public float health;
    public int damage = 10;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private bool isDead = false;

    private void Awake()
    {
        movement = FindAnyObjectByType<Movement>();
        player = movement.gameObject.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();

        // Set up NavMeshAgent for avoidance
        agent.avoidancePriority = Random.Range(0, 99); // Randomize priority to reduce chance of conflict
        agent.radius = 0.5f; // Adjust this to your desired personal space radius
        agent.height = 2.0f; // Adjust to the height of your enemy
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }

    private void Update()
    {
        if (isDead) return;

        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void ChasePlayer()
    {
        if (!player) return;
        agent.SetDestination(player.position);
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    private void AttackPlayer()
    {
        if (!player) return;
        if (!alreadyAttacked && health > 0)
        {
            // Make sure enemy doesn't move
            agent.speed = 0;

            transform.LookAt(player);

            animator.SetBool("isRunning", false);
            animator.SetTrigger("isAttacking"); // Play attack animation

            // The actual damage will be applied when the attack animation hits the player

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        if (isDead) return;

        alreadyAttacked = false;
        agent.speed = 3;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        agent.speed = 0;

        if (health <= 0)
        {
            isDead = true;
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
        else
        {
            animator.SetTrigger("hit");
            Invoke(nameof(ResetAttack), 2);
        }
    }

    private void DestroyEnemy()
    {
        animator.SetTrigger("die");
        Destroy(gameObject, 2.8f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // When the AttackCollider hits the player, apply damage
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
