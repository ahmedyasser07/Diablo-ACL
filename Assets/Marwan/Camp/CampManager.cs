using UnityEngine;
using UnityEngine.AI;

public class CampManager : MonoBehaviour
{
    [Header("Camp Waypoints")]
    [Tooltip("Assign your 4 waypoints here in the inspector")]
    public Transform[] waypoints; // Assign your 4 waypoints here in the Inspector

    [Header("Demon Settings")]
    [Tooltip("Assign the DemonAI prefab here")]
    public DemonAI demonPrefab; // Assign your DemonAI prefab here

    [Tooltip("Assign spawn points for Demons here")]
    public Transform[] spawnPoints; // Assign spawn points for Demons here

    [Header("Minion Settings")]
    [Tooltip("Assign the MinionAI prefab here")]
    public MinionAI minionPrefab; // Assign your MinionAI prefab here

    [Tooltip("Number of Minions to spawn")]
    public int numberOfMinions = 5; // Adjust as needed

    [Tooltip("Minimum distance from player to spawn minions")]
    public float minDistanceFromPlayer = 5f; // Prevent spawning too close to the player

    [Tooltip("Maximum attempts to find a valid spawn position")]
    public int maxSpawnAttempts = 10;

    [Header("Gizmo Settings")]
    [Tooltip("Color of the Gizmo lines and spheres")]
    public Color gizmoColor = Color.green; // Default Gizmo color

    [Tooltip("Size of the waypoint spheres")]
    public float gizmoSphereSize = 0.5f; // Size of the sphere at each waypoint

    void Start()
    {
        SpawnDemons();
        SpawnMinions();
    }

    /// <summary>
    /// Spawns Demons at specified spawn points and assigns references.
    /// </summary>
    void SpawnDemons()
    {
        if (demonPrefab == null)
        {
            Debug.LogError("Demon Prefab is not assigned in CampManager.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned in CampManager.");
            return;
        }

        foreach (Transform spawnPoint in spawnPoints)
        {
            // Instantiate Demon at each spawn point
            DemonAI demon = Instantiate(demonPrefab, spawnPoint.position, spawnPoint.rotation);

            // Assign the player reference (Assuming there's only one Player in the scene)
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats != null)
            {
                demon.player = playerStats.transform;
            }
            else
            {
                Debug.LogError("PlayerStats component not found in the scene.");
            }

            // Assign this CampManager to the Demon
            demon.campManager = this;
        }
    }

    /// <summary>
    /// Spawns Minions within the area defined by the four waypoints.
    /// </summary>
    void SpawnMinions()
    {
        if (minionPrefab == null)
        {
            Debug.LogError("Minion Prefab is not assigned in CampManager.");
            return;
        }

        if (waypoints == null || waypoints.Length != 4)
        {
            Debug.LogError("Exactly four waypoints must be assigned in CampManager to define the spawn area.");
            return;
        }

        // Calculate the bounds of the area defined by the four waypoints
        Bounds areaBounds = CalculateAreaBounds(waypoints);

        PlayerStats playerStatsInScene = FindObjectOfType<PlayerStats>();
        if (playerStatsInScene == null)
        {
            Debug.LogError("PlayerStats component not found in the scene.");
            return;
        }

        for (int i = 0; i < numberOfMinions; i++)
        {
            Vector3 randomPosition = Vector3.zero;
            bool positionFound = false;

            for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
            {
                // Generate a random position within the bounds
                randomPosition = new Vector3(
                    Random.Range(areaBounds.min.x, areaBounds.max.x),
                    areaBounds.center.y, // Assuming the area is horizontal
                    Random.Range(areaBounds.min.z, areaBounds.max.z)
                );

                // Ensure the position is a minimum distance away from the player
                if (Vector3.Distance(randomPosition, playerStatsInScene.transform.position) < minDistanceFromPlayer)
                {
                    continue; // Too close to the player, try another position
                }

                // Check if the position is on the NavMesh
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPosition, out hit, 2.0f, NavMesh.AllAreas))
                {
                    randomPosition = hit.position;
                    positionFound = true;
                    break; // Valid position found
                }
            }

            if (positionFound)
            {
                // Instantiate Minion at the valid random position
                MinionAI minion = Instantiate(minionPrefab, randomPosition, Quaternion.identity);

                // Assign the player reference
                minion.player = playerStatsInScene.transform;

                // Assign this CampManager to the Minion
                minion.campManager = this;
            }
            else
            {
                Debug.LogWarning($"Failed to find a valid spawn position for Minion {i + 1} after {maxSpawnAttempts} attempts.");
            }
        }
    }

    /// <summary>
    /// Calculates the bounding box of the area defined by the waypoints.
    /// Assumes that the waypoints form a roughly rectangular area.
    /// </summary>
    /// <param name="waypoints">Array of four waypoints.</param>
    /// <returns>Bounds representing the area.</returns>
    Bounds CalculateAreaBounds(Transform[] waypoints)
    {
        Vector3 min = waypoints[0].position;
        Vector3 max = waypoints[0].position;

        foreach (Transform waypoint in waypoints)
        {
            if (waypoint.position.x < min.x) min.x = waypoint.position.x;
            if (waypoint.position.y < min.y) min.y = waypoint.position.y;
            if (waypoint.position.z < min.z) min.z = waypoint.position.z;

            if (waypoint.position.x > max.x) max.x = waypoint.position.x;
            if (waypoint.position.y > max.y) max.y = waypoint.position.y;
            if (waypoint.position.z > max.z) max.z = waypoint.position.z;
        }

        return new Bounds((min + max) / 2, max - min);
    }

    
    
}
