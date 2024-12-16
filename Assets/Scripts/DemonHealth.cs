using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DemonHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int demonHealth = 40;
    public int CurrentHP;
    public bool IsDead { get; private set; } = false;

    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;

    public void Initialize(Animator anim, NavMeshAgent navAgent, Transform playerTarget)
    {
        animator = anim;
        agent = navAgent;
        player = playerTarget;
        CurrentHP = demonHealth;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        CurrentHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, demonHealth);
        Debug.Log($"Enemy took {amount} damage. CurrentHP: {CurrentHP}");

        if (CurrentHP <= 0 && !IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        if (agent != null)
            agent.isStopped = true;

        if (animator != null)
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
        yield return null; 
        float animationDuration = 0f;
        if (animator != null)
            animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animationDuration + 2.0f);
        Destroy(gameObject);
    }
}
