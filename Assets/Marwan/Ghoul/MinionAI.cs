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
    public int minion_health = 40;
    public int CurrentHP;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isPlayerInRange = false;

    private bool isDead = false;

    private bool isAttacking = false;
    public float attackCooldown = 1.5f; // Time between attacks


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        CurrentHP = minion_health;
        // Set the demon's walking speed
        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (isDead) return; // Prevent further updates if the minion is dead

        // if (CurrentHP <= 0)
        // {
        //     Die();
        //     return;
        // }


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
        animator.SetInteger ("moving", 2);//run
        agent.speed = runSpeed;
        agent.SetDestination(player.position);
        animator.Play("run");
    }

    void AttackPlayer()
    {
        // Prevent multiple attacks at once
        if (isAttacking) return;

        // Calculate distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Ensure the demon stops at a proper distance and faces the player
        if (distanceToPlayer <= attackRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Keep rotation only on the horizontal plane
            transform.rotation = Quaternion.LookRotation(direction); // Smoothly face the player

            agent.isStopped = true; // Stop further movement during the attack

            // Start the attack pattern
            isAttacking = true;
            StartCoroutine(PerformAttackPattern());
        }
    }
    




    IEnumerator PerformAttackPattern()
    {
        // Step 1: Swing the sword (first attack)
        animator.Play("attack1"); // Sword swing animation
        yield return new WaitForSeconds(attackCooldown);
        ApplyDamageToPlayer(10);
        // Step 2: Swing the sword again (second attack)
        animator.Play("attack2"); // Sword swing animation
        yield return new WaitForSeconds(attackCooldown);
        ApplyDamageToPlayer(10);
        // Step 3: Throw an explosive (cast spell)
        animator.Play("power_attack"); // Explosive animation
        yield return new WaitForSeconds(attackCooldown);
        ApplyDamageToPlayer(10);
        // Allow attacking again
        isAttacking = false;
    }

    void ApplyDamageToPlayer(int damageAmount)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(damageAmount);
        }
    }


    void Die()
    {
        isDead = true; // Prevent further actions
        agent.isStopped = true; // Stop movement
        animator.Play("Death"); // Play the dying animation
         PlayerStats playerStats = player.GetComponent<PlayerStats>();
    if (playerStats != null)
    {
        playerStats.GainXP(10);
        Debug.Log("Player gained 30 XP for killing an enemy.");
    }
        StartCoroutine(RemoveAfterAnimation());
    }

    IEnumerator RemoveAfterAnimation()
    {
        // Wait for the duration of the dying animation
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationDuration + 2.0f); // Adds an extra 2 seconds before disappearing

        // Destroy the demon game object
        Destroy(gameObject);
    }

     public void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, minion_health);
        Debug.Log($"Enemy took {amount} damage. CurrentHP: {CurrentHP}");

        if (CurrentHP <= 0 && !isDead)
        {
            Die();
        }
    }


}
