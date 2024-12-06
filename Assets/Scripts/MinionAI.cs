using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public float detectionRange = 10f; // Range to detect the player
    public float attackRange = 2f; // Range to attack the player
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public static int minion_health = 50;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isPlayerInRange = false;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Set the demon's walking speed
        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (isDead) return; // Prevent further updates if the minion is dead

        if (minion_health <= 0)
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
        else
        {
            Idle();
        }
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.Play("idle");
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.speed = runSpeed;
        agent.SetDestination(player.position);
        animator.Play("run");
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        transform.LookAt(player); // Face the player
        animator.Play("attack1");
    }

    void Die()
    {
        isDead = true; // Prevent further actions
        agent.isStopped = true; // Stop movement
        animator.Play("die"); // Play the dying animation
        StartCoroutine(RemoveAfterAnimation());
    }

    IEnumerator RemoveAfterAnimation()
    {
        // Wait for the duration of the dying animation
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationDuration);

        // Destroy the minion game object
        Destroy(gameObject);
    }
}
