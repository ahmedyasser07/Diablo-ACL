using System.Collections;
using System.Collections.Generic;
using Retro.ThirdPersonCharacter;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : BaseCharacter 
{
    public Transform player; // Assign the player in the Inspector
    public float detectionRange = 10f; // Range to detect the player
    public float attackRange = 2f; // Range to attack the player
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public int demon_health = 40;
    public int CurrentHP = 0 ;
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

        // Set the demon's walking speed
        CurrentHP = demon_health;
        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (isDead) return; // Prevent further updates if the minion is dead

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
    // Step 1: Sword attack
    animator.Play("Attack");
    yield return new WaitForSeconds(0.5f); // Wait for animation to "connect"
    ApplyDamageToPlayer(10); // Example damage value

    yield return new WaitForSeconds(attackCooldown);

    // Step 2: Another sword attack
    animator.Play("Attack");
    yield return new WaitForSeconds(0.5f);
    ApplyDamageToPlayer(10);

    yield return new WaitForSeconds(attackCooldown);

    // Step 3: Explosive spell
    animator.Play("Cast Spell");
    yield return new WaitForSeconds(1f); // Wait until spell "hits"
    ApplyDamageToPlayer(20);

    yield return new WaitForSeconds(attackCooldown);
    isAttacking = false;
}
void ApplyDamageToPlayer(int damageAmount)
{
    IDamageable damageableTarget = player.GetComponent<IDamageable>();
    if (damageableTarget != null)
    {
        damageableTarget.TakeDamage(damageAmount);
    }
}


    protected override void Die()
    {
        isDead = true; // Prevent further actions
        agent.isStopped = true; // Stop movement
        animator.Play("Death"); // Play the dying animation
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
     public override void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, demon_health);
        Debug.Log($"Enemy took {amount} damage. CurrentHP: {CurrentHP}");

        if (CurrentHP <= 0 && !isDead)
        {
            Die();
        }
    }


}