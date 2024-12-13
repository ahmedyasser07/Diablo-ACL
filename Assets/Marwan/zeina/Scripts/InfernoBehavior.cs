using System.Collections;
using UnityEngine;

public class InfernoBehavior : MonoBehaviour
{
    private float duration;
    private int initialDamage;
    private int damagePerSecond;
    private float radius;

    public void Initialize(float duration, int initialDamage, int damagePerSecond, float radius)
    {
        this.duration = duration;
        this.initialDamage = initialDamage;
        this.damagePerSecond = damagePerSecond;
        this.radius = radius;

        StartCoroutine(HandleInferno());
    }

    private IEnumerator HandleInferno()
    {
        // Initial damage
        ApplyDamage(initialDamage);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(1f);
            ApplyDamage(damagePerSecond);
            elapsedTime += 1f;
        }

        // Destroy Inferno after duration
        Destroy(gameObject);
        Debug.Log("Inferno expired.");
    }

    private void ApplyDamage(int damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Enemy"));
        foreach (Collider hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Enemy {enemy.name} takes {damage} damage from Inferno.");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
