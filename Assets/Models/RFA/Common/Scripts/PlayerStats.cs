using System.Collections.Generic;
using UnityEngine;

namespace Retro.ThirdPersonCharacter
{
    public class PlayerStats : MonoBehaviour
    {
        // Leveling variables
        public int Level { get; private set; } = 1;
        public int CurrentXP { get; private set; } = 0;
        public int XPToNextLevel { get; private set; } = 100;

        // Health variables
        public int MaxHP { get; private set; } = 100;
        public int CurrentHP { get; private set; }

        // Ability points
        public int AbilityPoints { get; private set; } = 0;

        // Unlocked abilities
        private HashSet<string> unlockedAbilities = new HashSet<string>();

        private void Start()
        {
            CurrentHP = MaxHP;
        }

        // Method to gain XP
        public void GainXP(int amount)
        {
            if (Level >= 4)
            {
                return; // Cannot gain XP beyond level 4
            }

            CurrentXP += amount;

            while (CurrentXP >= XPToNextLevel && Level < 4)
            {
                int overflowXP = CurrentXP - XPToNextLevel;
                LevelUp();
                CurrentXP = overflowXP;
            }
        }

        // Level up method
        private void LevelUp()
        {
            Level++;
            AbilityPoints++;

            MaxHP += 100;
            CurrentHP = MaxHP;

            XPToNextLevel = 100 * Level;

            // If level is 4 or more, set XP to 0 and prevent further leveling
            if (Level >= 4)
            {
                CurrentXP = 0;
            }
        }

        // Method to unlock abilities
        public bool UnlockAbility(string abilityName)
        {
            if (AbilityPoints <= 0)
            {
                return false; // Not enough ability points
            }

            if (unlockedAbilities.Contains(abilityName))
            {
                return false; // Ability already unlocked
            }

            unlockedAbilities.Add(abilityName);
            AbilityPoints--;
            return true;
        }

        // Check if ability is unlocked
        public bool IsAbilityUnlocked(string abilityName)
        {
            return unlockedAbilities.Contains(abilityName);
        }

        // Method to take damage
        public void TakeDamage(int amount)
        {
            CurrentHP -= amount;
            CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);

            if (CurrentHP <= 0)
            {
                Die();
            }
        }

        // Method to heal
        public void Heal(int amount)
        {
            CurrentHP += amount;
            CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        }

        // Handle player death
        private void Die()
        {
            // Implement game over logic here
            Debug.Log("Player has died.");
            // For example, you can load a game over scene or display a game over UI
        }
    }
}
