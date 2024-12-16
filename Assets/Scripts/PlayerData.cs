using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public int Level;
    public int CurrentXP;
    public int XPToNextLevel;
    public int MaxHP;
    public int CurrentHP;
    public int AbilityPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("PlayerData initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void SavePlayerStats(PlayerStats stats)
    {
        Level = stats.Level;
        CurrentXP = stats.CurrentXP;
        XPToNextLevel = stats.XPToNextLevel;
        MaxHP = stats.MaxHP;
        CurrentHP = stats.CurrentHP;
        AbilityPoints = stats.AbilityPoints;
    }

    public void LoadPlayerStats(PlayerStats stats)
    {
        stats.Level = Level;
        stats.CurrentXP = CurrentXP;
        stats.XPToNextLevel = XPToNextLevel;
        stats.MaxHP = MaxHP;
        stats.CurrentHP = CurrentHP;
        stats.AbilityPoints = AbilityPoints;
    }
}
