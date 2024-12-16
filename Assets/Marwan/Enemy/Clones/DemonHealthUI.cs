using UnityEngine;
using UnityEngine.UI;

public class DemonHealthUI : MonoBehaviour
{
    public DemonHealth demonHealth; // Reference to the demon's health script
    public Slider healthSlider;     // Reference to the slider UI

    void Start()
    {
        // Initialize the slider values
        if (demonHealth != null && healthSlider != null)
        {
            healthSlider.maxValue = demonHealth.demonHealth;
            healthSlider.value = demonHealth.CurrentHP;
        }
    }

    void Update()
    {
        // Continuously update slider value with current demon HP
        if (demonHealth != null && healthSlider != null)
        {
            healthSlider.value = demonHealth.CurrentHP;
        }
    }
}
