using UnityEngine;
using UnityEngine.UI;

namespace Retro.ThirdPersonCharacter
{
    public class AbilityUI : MonoBehaviour
    {
        [SerializeField] private AbilityManager abilityManager;
        [SerializeField] private Button unlockAbilityButton;
        [SerializeField] private Text abilityNameText;

        private string abilityName = "SpecialAbility";

        private void Start()
        {
            abilityNameText.text = abilityName;
            unlockAbilityButton.onClick.AddListener(UnlockAbility);
        }

        private void UnlockAbility()
        {
            abilityManager.UnlockAbility(abilityName);
        }
    }
}
