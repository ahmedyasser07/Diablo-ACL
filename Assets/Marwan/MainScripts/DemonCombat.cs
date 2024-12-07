using System.Collections;
using UnityEngine;

public class DemonCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 1.5f;
    public bool isAttacking = false;

    private Animator animator;
    private Transform player;
    private DemonHealth health;

    public void Initialize(Animator anim, Transform playerTarget)
    {
        this.animator = anim;
        this.player = playerTarget;
        this.health = GetComponent<DemonHealth>(); // Get reference to health
    }

    public void TryAttackPlayer(float attackRange)
    {
        if (isAttacking) return;

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
        // Before each action, check if demon is dead
        if (health.IsDead) { isAttacking = false; yield break; }

        // First attack
        animator.Play("Attack");
        yield return new WaitForSeconds(0.5f);
        if (health.IsDead) { isAttacking = false; yield break; }
        ApplyDamageToPlayer(10);

        yield return new WaitForSeconds(attackCooldown);
        if (health.IsDead) { isAttacking = false; yield break; }

        // Second attack
        animator.Play("Attack");
        yield return new WaitForSeconds(0.5f);
        if (health.IsDead) { isAttacking = false; yield break; }
        ApplyDamageToPlayer(10);

        yield return new WaitForSeconds(attackCooldown);
        if (health.IsDead) { isAttacking = false; yield break; }

        // Cast Spell attack
        animator.Play("Cast Spell");
        yield return new WaitForSeconds(1f);
        if (health.IsDead) { isAttacking = false; yield break; }
        ApplyDamageToPlayer(20);

        yield return new WaitForSeconds(attackCooldown);
        // Final check after last attack
        if (health.IsDead) { isAttacking = false; yield break; }

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
}
