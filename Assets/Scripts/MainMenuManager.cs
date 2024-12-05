using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Replace with your game scene name
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
}
