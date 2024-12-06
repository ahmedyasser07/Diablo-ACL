using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemonAI : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public float detectionRange = 10f; // Range to detect the player
    public float attackRange = 2f; // Range to attack the player
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public int demon_health = 40;
    public Transform[] patrolPoints; // Waypoints for patrolling
    public float patrolWaitTime = 2f; // Time to wait at each patrol point

    private NavMeshAgent agent;
    private Animator animator;
    private bool isPlayerInRange = false;
    private bool isDead = false;
    private bool isAttacking = false;
    private bool isWaiting = false;
    private int currentPatrolPoint = 0;
    public float attackCooldown = 1.5f; // Time between attacks

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Set the demon's walking speed
        agent.speed = walkSpeed;

        // Start patrolling if patrol points are set
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolPoint].position);
            animator.Play("Walk");
        }
    }

    void Update()
    {
        if (isDead) return; // Prevent further updates if the demon is dead

        if (demon_health <= 0)
        {
            Die();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (patrolPoints.Length > 0) // Patrol if the player is out of range
        {
            Patrol();
        }
        else
        {
            Idle();
        }
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.Play("Idle");
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.speed = runSpeed;
        agent.SetDestination(player.position);
        animator.Play("Run");
    }

    void AttackPlayer()
    {
        if (isAttacking) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);

            agent.isStopped = true;
            isAttacking = true;
            StartCoroutine(PerformAttackPattern());
        }
    }

    IEnumerator PerformAttackPattern()
    {
        animator.Play("Attack");
        yield return new WaitForSeconds(attackCooldown);

        animator.Play("Attack");
        yield return new WaitForSeconds(attackCooldown);

        animator.Play("Cast Spell");
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    void Patrol()
    {
        if (agent.remainingDistance < 0.5f && !isWaiting)
        {
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        animator.Play("Idle");
        yield return new WaitForSeconds(patrolWaitTime);
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        animator.Play("Walk");
        isWaiting = false;
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.Play("Death");
        StartCoroutine(RemoveAfterAnimation());
    }

    IEnumerator RemoveAfterAnimation()
    {
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationDuration + 2.0f);
        Destroy(gameObject);
    }
}
