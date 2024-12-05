using UnityEngine;

public class RotateCharacter : MonoBehaviour
{
    public float rotationSpeed = 50f;

    void Update()
    {
        // Rotate the character around its local Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }
}
