using UnityEngine;

public class CampManager : MonoBehaviour
{
    [Header("Camp Waypoints")]
    public Transform[] waypoints; // Assign your 4 waypoints here in the inspector

    [Header("Demon Prefab")]
    public DemonAI demonPrefab; // Drag your Demon prefab here

    private DemonAI spawnedDemon;

    void Start()
    {
        // If a prefab is assigned, instantiate the demon
        if (demonPrefab != null)
        {
            spawnedDemon = Instantiate(demonPrefab, transform.position, Quaternion.identity);
            AssignWaypointsToDemon(spawnedDemon);
        }
    }

    void AssignWaypointsToDemon(DemonAI demon)
    {
        if (demon != null && waypoints != null && waypoints.Length > 0)
        {
            demon.waypoints = waypoints;
        }
    }
}
