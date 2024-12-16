using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Drag the player object here in the Inspector
    public Vector3 offset = new Vector3(0, 10, -10); // Adjust offset as needed

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
            transform.LookAt(player); // Keeps the camera focused on the player
        }
    }
}