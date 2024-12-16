using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    public void SelectBarbarian()
    {
        PlayerSelectionManager.SelectedPlayer = "Barbarian";
        SceneManager.LoadScene("MainScene");
    }

    public void SelectSorcerer()
    {
        PlayerSelectionManager.SelectedPlayer = "Sorcerer";
        SceneManager.LoadScene("MainScene");
    }
}
