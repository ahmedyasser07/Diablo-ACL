using UnityEngine;

namespace Retro.ThirdPersonCharacter
{
    public class AbilityManager : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;

        public void UnlockAbility(string abilityName)
        {
            bool success = playerStats.UnlockAbility(abilityName);
            if (success)
            {
                Debug.Log($"{abilityName} unlocked!");
            }
            else
            {
                Debug.Log($"Failed to unlock {abilityName}.");
            }
        }

        public bool IsAbilityUnlocked(string abilityName)
        {
            return playerStats.IsAbilityUnlocked(abilityName);
        }

        // Implement the abilities here
        public void UseAbility(string abilityName)
        {
            if (IsAbilityUnlocked(abilityName))
            {
                // Perform ability action
                Debug.Log($"Using ability: {abilityName}");
            }
            else
            {
                Debug.Log($"{abilityName} is not unlocked.");
            }
        }
    }
}
