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

        private void Start()
        {
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
            float fillAmount = (float)playerStats.CurrentHP / playerStats.MaxHP;
            healthBarFill.fillAmount = fillAmount;
            healthBarText.text = $"{playerStats.CurrentHP} / {playerStats.MaxHP}";
        }

        private void UpdateXPBar()
        {
            float fillAmount = (float)playerStats.CurrentXP / playerStats.XPToNextLevel;
            xpBarFill.fillAmount = fillAmount;
            xpBarText.text = $"{playerStats.CurrentXP} / {playerStats.XPToNextLevel}";
        }

        private void UpdateLevelDisplay()
        {
            levelText.text = $"Level {playerStats.Level}";
        }

        private void UpdateAbilityPointsDisplay()
        {
            abilityPointsText.text = $"Ability Points: {playerStats.AbilityPoints}";
        }
    }
}
