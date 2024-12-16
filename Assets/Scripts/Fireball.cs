using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;         // Speed of the fireball
    public int damage = 5;           // Damage dealt by the fireball
    private Transform target;        // Target enemy
    public GameObject explosionEffect; // Explosion effect prefab

    // Set the target for the Fireball
    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    void Update()
    {
        if (target == null)
        {
            // Destroy the fireball if the target is null
            Destroy(gameObject);
            return;
        }

        // Move the fireball toward the target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Rotate to face the target
        transform.LookAt(target);

        // Check if the fireball is close enough to the target
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        // Apply damage to the target if it has an Enemy component
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Trigger explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Destroy the fireball
        Destroy(gameObject);
    }
}
