using UnityEngine;

/// <summary>
/// Draws Gizmos for waypoints managed by the CampManager.
/// Highlights each waypoint with a unique color and connects them with lines.
/// Attach this script to the CampManager GameObject.
/// </summary>
[ExecuteAlways] // Ensures Gizmos are drawn in edit mode as well
public class Highlighter : MonoBehaviour
{
    [Header("Camp Manager Reference")]
    [Tooltip("Assign the CampManager GameObject here.")]
    public CampManager campManager; // Reference to the CampManager script

    [Header("Gizmo Settings")]
    [Tooltip("Enable to draw lines connecting waypoints.")]
    public bool drawLines = true;

    [Tooltip("Size of the waypoint spheres.")]
    public float sphereSize = 0.5f;

    [Tooltip("Colors for each waypoint.")]
    public Color[] waypointColors; // Array of colors for waypoints

    private void OnDrawGizmos()
    {
        if (campManager == null)
        {
            // Attempt to find the CampManager on the same GameObject if not assigned
            campManager = GetComponent<CampManager>();
            if (campManager == null)
            {
                Debug.LogWarning("WaypointsGizmos: CampManager reference is missing.");
                return;
            }
        }

        Transform[] waypoints = campManager.waypoints;

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("WaypointsGizmos: No waypoints assigned in CampManager.");
            return;
        }

        // Ensure the waypointColors array has enough colors
        if (waypointColors == null || waypointColors.Length < waypoints.Length)
        {
            // If not enough colors, initialize with random colors
            waypointColors = new Color[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypointColors[i] = Random.ColorHSV();
            }
        }

        // Draw spheres at each waypoint
        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform waypoint = waypoints[i];
            if (waypoint != null)
            {
                Gizmos.color = waypointColors[i];
                Gizmos.DrawSphere(waypoint.position, sphereSize);

                // Optionally, label the waypoint
                #if UNITY_EDITOR
                UnityEditor.Handles.Label(waypoint.position + Vector3.up * sphereSize, $"Waypoint {i + 1}");
                #endif
            }
            else
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(transform.position, sphereSize);
            }
        }

        // Draw lines connecting waypoints
        if (drawLines && waypoints.Length > 1)
        {
            Gizmos.color = Color.red; // Set line color

            for (int i = 0; i < waypoints.Length; i++)
            {
                Transform currentWaypoint = waypoints[i];
                Transform nextWaypoint = waypoints[(i + 1) % waypoints.Length]; // Loop back to first waypoint

                if (currentWaypoint != null && nextWaypoint != null)
                {
                    Gizmos.DrawLine(currentWaypoint.position, nextWaypoint.position);
                }
            }
        }
    }
}
