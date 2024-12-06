using UnityEngine;

namespace Retro.ThirdPersonCharacter
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target; // Reference to the player character
        [SerializeField] private float _distance = 10.0f; // Horizontal distance from the target
        [SerializeField] private float _height = 10.0f; // Vertical height above the target
        [SerializeField] private float _angle = 45.0f; // Angle at which the camera looks down

        private void Start()
        {
            if (_target == null)
            {
                Debug.LogError("CameraController: No target set for the camera.");
                return;
            }

            // Set the initial position and rotation of the camera
            UpdateCameraPosition();
        }

        private void LateUpdate()
        {
            // Update the position and rotation of the camera every frame
            UpdateCameraPosition();
        }

        private void UpdateCameraPosition()
        {
            // Calculate the offset based on the angle
            Quaternion rotation = Quaternion.Euler(_angle, 0, 0);
            Vector3 offset = rotation * new Vector3(0, 0, -_distance);

            // Set the camera's position and look at the target
            transform.position = _target.position + Vector3.up * _height + offset;
            transform.LookAt(_target.position + Vector3.up * _height);
        }
    }
}
