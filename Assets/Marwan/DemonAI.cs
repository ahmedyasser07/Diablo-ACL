using UnityEngine;
using UnityEngine.AI;

public class DemonAI : MonoBehaviour
{
    // References
    [Header("References")]
    [Tooltip("Assign the player Transform here")]
    public Transform player; // Assigned by CampManager

    [Tooltip("Reference to the CampManager")]
    public CampManager campManager; // Assigned by CampManager

    private NavMeshAgent agent;
    private Animator animator;
    private DemonCombat combat;
    private DemonHealth health;

    [Header("Movement Settings")]
    [Tooltip("Range to detect the player")]
    public float detectionRange = 10f;

    [Tooltip("Range to attack the player")]
    public float attackRange = 2f;

    [Tooltip("Walking speed of the Demon")]
    public float walkSpeed = 2f;

    [Tooltip("Running speed of the Demon")]
    public float runSpeed = 4f;

    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        combat = GetComponent<DemonCombat>();
        health = GetComponent<DemonHealth>();

        // Initialize health and combat scripts with references they need
        health.Initialize(animator, agent, player);
        combat.Initialize(animator, player);

        agent.speed = walkSpeed;
    }

    void Update()
    {
        // If dead, do nothing
        if (health.IsDead)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange)
        {
            // Player is too far, patrol around waypoints
            Patrol();
        }
        else if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            // Player in detection range but not in attack range, chase player
            ChasePlayer();
        }
        else if (distanceToPlayer <= attackRange)
        {
            // Player within attack range, attack
            combat.TryAttackPlayer(attackRange);
        }
        else
        {
            // Default fallback to idle if something unexpected
            Idle();
        }
    }

    void Idle()
    {
        if (!health.IsDead)
        {
            agent.isStopped = true;
            animator.Play("Idle");
        }
    }

    void ChasePlayer()
    {
        if (!health.IsDead)
        {
            agent.isStopped = false;
            agent.speed = runSpeed;
            agent.SetDestination(player.position);
            animator.Play("Run");
        }
    }

    void Patrol()
    {
        if (!health.IsDead)
        {
            // Ensure CampManager and waypoints are assigned
            if (campManager == null || campManager.waypoints == null || campManager.waypoints.Length == 0)
            {
                Idle();
                return;
            }

            agent.isStopped = false;
            agent.speed = walkSpeed;

            // Set destination to current waypoint
            agent.SetDestination(campManager.waypoints[currentWaypointIndex].position);

            // If close to the waypoint, move to the next one
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % campManager.waypoints.Length;
            }

            animator.Play("Walk");
        }
    }

    // When taking damage, just forward the call to DemonHealth
    public void TakeDamage(int amount)
    {
        health.TakeDamage(amount);
    }
}
