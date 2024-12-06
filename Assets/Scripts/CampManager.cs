using UnityEngine;

public class CampManager : MonoBehaviour
{
    public GameObject campPrefab; // The camp prefab to spawn
    public int numberOfCamps = 2; // Total camps to spawn
    public float distanceBetweenCamps = 50f; // Distance between camps

    void Start()
    {
        for (int i = 0; i < numberOfCamps; i++)
        {
            Debug.Log($"Spawning camp {i + 1}");
            Vector3 campPosition = new Vector3(0, 0, distanceBetweenCamps++);
            Instantiate(campPrefab, campPosition, Quaternion.identity);
        }
    }
}
