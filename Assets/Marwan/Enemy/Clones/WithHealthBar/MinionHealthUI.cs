using UnityEngine;
using UnityEngine.UI;

public class MinionHealthUI : MonoBehaviour
{
    public MinionHealth minionHealth; // Reference to the demon's health script
    public Slider healthSlider;     // Reference to the slider UI

    void Start()
    {
        // Initialize the slider values
        if (minionHealth != null && healthSlider != null)
        {
            healthSlider.maxValue = minionHealth.minionHealth;
            healthSlider.value = minionHealth.CurrentHP;
        }
    }

    void Update()
    {
        // Continuously update slider value with current demon HP
        if (minionHealth != null && healthSlider != null)
        {
            healthSlider.value = minionHealth.CurrentHP;
        }
    }
}
