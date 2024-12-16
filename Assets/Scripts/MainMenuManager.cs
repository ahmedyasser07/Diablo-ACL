using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("ChooseCharacterScene"); // Replace with your game scene name
    }

    public void BackToMenus()
    {
        SceneManager.LoadScene("MainMenuScene"); // Replace with your game scene name
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsScene"); // Placeholder for options functionality
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game quit"); // Logs quit action in the editor
    }

    public void RestartLevel()
    {
        // Reload the current scene
        string currentScene = "MainScene";
        SceneManager.LoadScene(currentScene);

        // Restore player stats after reloading
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null && PlayerData.Instance != null)
        {
            PlayerData.Instance.LoadPlayerStats(playerStats);
        }
    }

}
