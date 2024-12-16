using System.Collections;
using UnityEngine;

public class MinionCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 1.5f;
    public bool isAttacking = false;

    private Animator animator;
    private Transform player;
    private MinionHealth health;
    private float attackRange;

    public void Initialize(Animator anim, Transform playerTarget, float atkRange)
    {
        this.animator = anim;
        this.player = playerTarget;
        this.health = GetComponent<MinionHealth>();
        this.attackRange = atkRange;
    }

    public void TryAttackPlayer()
    {
        if (isAttacking || health.IsDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Face the player
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);

            // Start attacking coroutine
            isAttacking = true;
            StartCoroutine(PerformAttackPattern());
        }
    }

    IEnumerator PerformAttackPattern()
    {
        // Check if dead before starting
        if (health.IsDead) { isAttacking = false; yield break; }

        // Step 1: Swing (attack1)
        animator.Play("attack1");
        yield return new WaitForSeconds(attackCooldown);
        if (health.IsDead) { isAttacking = false; yield break; }
        ApplyDamageToPlayer(10);

        // Step 2: Swing (attack2)
        animator.Play("attack2");
        yield return new WaitForSeconds(attackCooldown);
        if (health.IsDead) { isAttacking = false; yield break; }
        ApplyDamageToPlayer(10);

        // Step 3: Power attack (power_attack)
        animator.Play("power_attack");
        yield return new WaitForSeconds(attackCooldown);
        if (health.IsDead) { isAttacking = false; yield break; }
        ApplyDamageToPlayer(10);

        isAttacking = false;
    }

    void ApplyDamageToPlayer(int damageAmount)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null && !health.IsDead)
        {
            playerStats.TakeDamage(damageAmount);
        }
    }
}
