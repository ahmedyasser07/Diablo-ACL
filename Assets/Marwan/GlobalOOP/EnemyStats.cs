using UnityEngine;
using Retro.ThirdPersonCharacter;

[RequireComponent(typeof(Animator))]
public class EnemyStats : BaseCharacter, IDamageable
{
    public int MaxHP = 40;
    public int CurrentHP;

    private Animator animator;
    private bool isDead = false;

    private void Start()
    {
        CurrentHP = MaxHP;
        animator = GetComponent<Animator>();
    }

    public override void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        Debug.Log($"Enemy took {amount} damage. CurrentHP: {CurrentHP}");

        if (CurrentHP <= 0 && !isDead)
        {
            Die();
        }
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;
        GetComponent<EnemyAi>().enabled = false;
        animator.SetTrigger("Die");
        Debug.Log("Enemy has died.");

        // Optionally destroy the enemy after a delay
        Destroy(gameObject, 3f);
    }
}
