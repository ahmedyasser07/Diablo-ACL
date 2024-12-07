using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Retro.ThirdPersonCharacter;
public class PlayerStats : MonoBehaviour
{
    public int Level = 1;
    public int CurrentXP = 0;
    public int XPToNextLevel = 100;

    public int MaxHP = 100;
    public int CurrentHP;

    public int AbilityPoints = 0;
    //private HashSet<string> unlockedAbilities = new HashSet<string>();
    private bool isDead = false;

    private void Start()
    {
        CurrentHP = MaxHP;
        Debug.Log("PlayerStats initialized. HP: " + CurrentHP);
    }

    public void GainXP(int amount)
    {
        if (Level >= 4)
        {
            Debug.Log("Max level reached. No more XP can be gained.");
            return;
        }

        CurrentXP += amount;
        Debug.Log($"Gained {amount} XP. Current XP: {CurrentXP}, XPToNextLevel: {XPToNextLevel}");

        while (CurrentXP >= XPToNextLevel && Level < 4)
        {
            int overflowXP = CurrentXP - XPToNextLevel;
            LevelUp();
            CurrentXP = overflowXP;
        }
    }

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
            CurrentXP = 0;
            Debug.Log("Reached max level (4). No further leveling possible.");
        }
    }

   

    public void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        Debug.Log($"Player took {amount} damage. CurrentHP: {CurrentHP}");

        if (CurrentHP <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        Debug.Log($"Player healed for {amount}. CurrentHP: {CurrentHP}");
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        GetComponent<PlayerInput>().enabled = false;
        GetComponent<Combat>().enabled = false;

        GetComponent<Animator>().SetTrigger("Die");
        Debug.Log("Player has died.");
    }
}



 // public bool UnlockAbility(string abilityName)
    // {
    //     if (AbilityPoints <= 0)
    //     {
    //         Debug.Log("Not enough Ability Points to unlock ability: " + abilityName);
    //         return false;
    //     }

    //     if (unlockedAbilities.Contains(abilityName))
    //     {
    //         Debug.Log("Ability already unlocked: " + abilityName);
    //         return false;
    //     }

    //     unlockedAbilities.Add(abilityName);
    //     AbilityPoints--;
    //     Debug.Log("Unlocked ability: " + abilityName);
    //     return true;
    // }

    // public bool IsAbilityUnlocked(string abilityName)
    // {
    //     return unlockedAbilities.Contains(abilityName);
    // }