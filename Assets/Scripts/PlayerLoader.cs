using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public GameObject barbarian;     // Assign "Player-Default" in the Inspector
    public GameObject sorcerer;      // Assign "Nightshade J Friedrich@Idle" in the Inspector
    public Camera mainCamera;        // Assign the "Main Camera" for Barbarian in the Inspector
    public Camera secondaryCamera;   // Assign the "Main Camera2" for Sorcerer in the Inspector

    private void Start()
    {
        if (PlayerSelectionManager.SelectedPlayer == "Barbarian")
        {
            // Enable Barbarian and its camera
            barbarian.SetActive(true);
            sorcerer.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            secondaryCamera.gameObject.SetActive(false);
        }
        else if (PlayerSelectionManager.SelectedPlayer == "Sorcerer")
        {
            // Enable Sorcerer and its camera
            barbarian.SetActive(false);
            sorcerer.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            secondaryCamera.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No player selected! Defaulting to Barbarian.");
            // Default to Barbarian
            barbarian.SetActive(true);
            sorcerer.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            secondaryCamera.gameObject.SetActive(false);
        }
    }
}
