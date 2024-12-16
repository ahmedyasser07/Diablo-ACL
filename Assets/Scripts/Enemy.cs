using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 20; // Enemy health

    // Method to apply damage to the enemy
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage!");

        // Check if the enemy's health is depleted
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to handle the enemy's death
    private void Die()
    {
        Debug.Log(gameObject.name + " has been defeated!");
        Destroy(gameObject); // Destroy the enemy GameObject
    }
}
