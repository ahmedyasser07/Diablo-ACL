using UnityEngine;

namespace Retro.ThirdPersonCharacter
{
    public class PlayerInput : MonoBehaviour
    {
        private bool _attackInput;
        private bool _specialAttackInput;
        private Vector2 _movementInput;
        private bool _jumpInput;

        public bool AttackInput { get => _attackInput; }
        public bool SpecialAttackInput { get => _specialAttackInput; }
        public Vector2 MovementInput { get => _movementInput; }
        public bool JumpInput { get => _jumpInput; }

        private Vector3 _targetPosition;
        private bool _isMoving = false;

        private void Update()
        {
            // Update attack inputs to 'Q' and 'W' keys
            _attackInput = Input.GetKeyDown(KeyCode.Q);
            _specialAttackInput = Input.GetKeyDown(KeyCode.W);

            // Handle point-and-click movement
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    _targetPosition = hit.point;
                    _isMoving = true;
                }
            }

            if (_isMoving)
            {
                Vector3 direction = (_targetPosition - transform.position).normalized;
                _movementInput = new Vector2(direction.x, direction.z);

                // Stop moving if close to the target position
                if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
                {
                    _isMoving = false;
                    _movementInput = Vector2.zero;
                }
            }
            else
            {
                _movementInput = Vector2.zero;
            }

            // Keep jump input as it was
            _jumpInput = Input.GetButton("Jump");
        }
    }
}
