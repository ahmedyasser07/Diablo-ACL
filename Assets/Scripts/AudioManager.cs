using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Slider musicSlider;      // Slider for background music
    public Slider sfxSlider;        // Slider for sound effects
    public AudioSource musicSource; // AudioSource for background music
    public AudioSource sfxSource;   // AudioSource for sound effects

    void Start()
    {
        // Set the initial values based on the sliders' values
        musicSource.volume = musicSlider.value;
        sfxSource.volume = sfxSlider.value;

        // Add listeners to update volumes when the sliders are changed
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Method to update the background music volume
    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    // Method to update the sound effects volume
    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
    }

    public void BackToMenus()
    {
        SceneManager.LoadScene("MainMenuScene"); // Replace with your game scene name
    }
}
