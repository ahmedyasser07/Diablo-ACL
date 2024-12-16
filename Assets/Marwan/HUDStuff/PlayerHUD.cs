using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Retro.ThirdPersonCharacter
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;

        // Health Bar UI elements
        [SerializeField] private Image healthBarFill;
        [SerializeField] private TextMeshProUGUI healthBarText;

        // XP Bar UI elements
        [SerializeField] private Image xpBarFill;
        [SerializeField] private TextMeshProUGUI xpBarText;

        // Level Display
        [SerializeField] private TextMeshProUGUI levelText;

        // Ability Points Display
        [SerializeField] private TextMeshProUGUI abilityPointsText;

        [SerializeField] private GameObject barbarian;
        [SerializeField] private GameObject sorcerer;

        private void Start()
        {
            // Check if the playerStats reference is valid, and if the assigned player object is active
            if (playerStats == null || !playerStats.gameObject.activeSelf)
            {
                // If playerStats is null or disabled, assign the playerStats to Sorcerer (Nightshade J Friedrich@Idle)
                if (sorcerer != null)
                {
                    playerStats = sorcerer.GetComponent<PlayerStats>();
                }
                else
                {
                    Debug.LogWarning("Sorcerer object not assigned.");
                }
            }

            // Now update the HUD
            UpdateHealthBar();
            UpdateXPBar();
            UpdateLevelDisplay();
            UpdateAbilityPointsDisplay();
        }

        private void Update()
        {
            UpdateHealthBar();
            UpdateXPBar();
            UpdateLevelDisplay();
            UpdateAbilityPointsDisplay();
        }

        private void UpdateHealthBar()
        {
            if (playerStats != null)
            {
                float fillAmount = (float)playerStats.CurrentHP / playerStats.MaxHP;
                healthBarFill.fillAmount = fillAmount;
                healthBarText.text = $"{playerStats.CurrentHP} / {playerStats.MaxHP}";
            }
        }

        private void UpdateXPBar()
        {
            if (playerStats != null)
            {
                float fillAmount = (float)playerStats.CurrentXP / playerStats.XPToNextLevel;
                xpBarFill.fillAmount = fillAmount;
                xpBarText.text = $"{playerStats.CurrentXP} / {playerStats.XPToNextLevel}";
            }
        }

        private void UpdateLevelDisplay()
        {
            if (playerStats != null)
            {
                levelText.text = $"Level {playerStats.Level}";
            }
        }

        private void UpdateAbilityPointsDisplay()
        {
            if (playerStats != null)
            {
                abilityPointsText.text = $"Ability Points: {playerStats.AbilityPoints}";
            }
        }
    }
}
