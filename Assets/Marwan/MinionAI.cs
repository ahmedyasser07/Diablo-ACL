using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
     [Tooltip("Reference to the CampManager")]
    public CampManager campManager; // Assigned by CampManager
    private NavMeshAgent agent;
    private Animator animator;
    private MinionCombat combat;
    private MinionHealth health;

    [Header("Movement Settings")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        combat = GetComponent<MinionCombat>();
        health = GetComponent<MinionHealth>();

        // Initialize health and combat with references
        health.Initialize(animator, agent, player);
        combat.Initialize(animator, player, attackRange);

        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (health.IsDead)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer <= attackRange)
        {
            // Ask combat script to handle attack logic
            combat.TryAttackPlayer();
        }
        else
        {
            Idle();
        }
    }

    void Idle()
    {
        if (!health.IsDead)
        {
            agent.isStopped = true;
            animator.Play("idle");
        }
    }

    void ChasePlayer()
    {
        if (!health.IsDead)
        {
            agent.isStopped = false;
            agent.speed = runSpeed;
            agent.SetDestination(player.position);
            animator.Play("run");
        }
    }

    // When taking damage, just forward the call to MinionHealth
    public void TakeDamage(int amount)
    {
        health.TakeDamage(amount);
    }
}
