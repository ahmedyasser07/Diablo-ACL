using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Retro.ThirdPersonCharacter;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]

public class PlayerStats : MonoBehaviour
{
    // The Wanderer starts at character level 1 with 0 XP.
    public int Level = 1;
    public int CurrentXP = 0;
    // XP required to reach the next level is 100 * current level.
    public int XPToNextLevel = 100;

    // Starting HP for level 1 is 100.
    public int MaxHP = 100;
    public int CurrentHP;
    public int healingPotions;

    // The Wanderer gains 1 ability point per level-up.
    public int AbilityPoints = 0;

    // When the Wanderer dies, this flag will be set.
    private bool isDead = false;


    private void Start()
    {
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.LoadPlayerStats(this);
        }
        else
        {
            // Default initialization if no data exists
            XPToNextLevel = 100 * Level; 
            CurrentHP = MaxHP-10;
        }
        healingPotions=0;
        Debug.Log("PlayerStats initialized. HP: " + CurrentHP);
    }


    // The Wanderer gains XP upon killing enemies or other events.
    public void GainXP(int amount)
    {
        // If the Wanderer is already at level 4, they cannot gain XP or level up further.
        if (Level >= 4)
        {
            Debug.Log("Max level reached. No more XP can be gained.");
            return;
        }

        CurrentXP += amount;
        Debug.Log($"Gained {amount} XP. Current XP: {CurrentXP}, XPToNextLevel: {XPToNextLevel}");

        // Check if we have enough XP to level up, and handle overflow XP as necessary.
        while (CurrentXP >= XPToNextLevel && Level < 4)
        {
            int overflowXP = CurrentXP - XPToNextLevel;
            LevelUp();
            CurrentXP = overflowXP;
        }
    }

    // Level up the Wanderer:
    // - Increase level by 1
    // - Gain 1 ability point
    // - Increase MaxHP by 100
    // - Refill CurrentHP to the new MaxHP
    // - Update XPToNextLevel to 100 * new Level
    // - Once level 4 is reached, no further levels can be gained.
    private void LevelUp()
    {
        Level++;
        AbilityPoints++;
        MaxHP += 100;
        CurrentHP = MaxHP;

        XPToNextLevel = 100 * Level;
        Debug.Log($"Leveled up! New Level: {Level}, HP: {CurrentHP}, Next XP threshold: {XPToNextLevel}");

        if (Level >= 4)
        {
            // Once level 4 is reached, reset current XP to 0 and prevent further XP gains.
            CurrentXP = 0;
            Debug.Log("Reached max level (4). No further leveling possible.");
        }
    }

    // The Wanderer can take damage. If HP reaches 0, Die() is called.
    public void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        Debug.Log($"Player took {amount} damage. CurrentHP: {CurrentHP}");

        if (CurrentHP <= 0 && !isDead)
        {
            Die();

            // Call GameOver with a delay
            StartCoroutine(DelayedGameOver(3f)); // 2 seconds delay
        }
    }


    // The Wanderer can heal up to their MaxHP.
    public void Heal()
    {
        CurrentHP = MaxHP;
        Debug.Log(CurrentHP);
        Debug.Log($"Player healed for maxHP . CurrentHP: {CurrentHP}");
    }

    // When the Wanderer dies, disable player input, combat, and trigger the "Die" animation.
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.SavePlayerStats(this);
        }
        else
        {
            Debug.LogWarning("PlayerData.Instance is null. Stats could not be saved.");
        }

        GetComponent<PlayerInput>().enabled = false;
        GetComponent<Combat>().enabled = false;

        GetComponent<Animator>().SetTrigger("Die");
        Debug.Log("Player has died.");
    }


    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScene"); // Placeholder for options functionality
    }


    private IEnumerator DelayedGameOver(float delay)
{
    yield return new WaitForSeconds(delay);
    GameOver();
}

private void OnTriggerEnter(Collider other){
         if (other.CompareTag("healing"))
        {
            if(healingPotions==3)return;
            if(healingPotions<3){
                healingPotions++;
                Destroy(other.gameObject);
            }
        }
}

}