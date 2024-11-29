using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private float introDuration = 3f; // Duration in seconds
    [SerializeField] private string nextScene = "MainMenuScene"; // Name of the next scene

    private void Start()
    {
        Invoke("LoadNextScene", introDuration);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
