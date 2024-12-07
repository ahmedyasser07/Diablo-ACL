using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public int demonHealth = 40;
    public int CurrentHP;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isDead = false;
    private bool isAttacking = false;
    public float attackCooldown = 1.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        CurrentHP = demonHealth;
        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (isDead) return;

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
        yield return new WaitForSeconds(0.5f);
        ApplyDamageToPlayer(10);

        yield return new WaitForSeconds(attackCooldown);

        animator.Play("Attack");
        yield return new WaitForSeconds(0.5f);
        ApplyDamageToPlayer(10);

        yield return new WaitForSeconds(attackCooldown);

        animator.Play("Cast Spell");
        yield return new WaitForSeconds(1f);
        ApplyDamageToPlayer(20);

        yield return new WaitForSeconds(attackCooldown);
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

    private void Die()
{
    isDead = true;
    agent.isStopped = true;
    animator.Play("Death");

    // Award the player XP here
    PlayerStats playerStats = player.GetComponent<PlayerStats>();
    if (playerStats != null)
    {
        playerStats.GainXP(30);
        Debug.Log("Player gained 30 XP for killing an enemy.");
    }

    StartCoroutine(RemoveAfterAnimation());
}


    IEnumerator RemoveAfterAnimation()
    {
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationDuration + 2.0f);
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, demonHealth);
        Debug.Log($"Enemy took {amount} damage. CurrentHP: {CurrentHP}");

        if (CurrentHP <= 0 && !isDead)
        {
            Die();
        }
    }
}
