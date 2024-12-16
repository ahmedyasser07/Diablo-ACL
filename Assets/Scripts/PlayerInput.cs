using UnityEngine;
using Retro.ThirdPersonCharacter;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerHUD))]


public class PlayerInput : MonoBehaviour
{
    private Vector2 _movementInput;
    private Vector3 _targetPosition;
    private bool _isMoving;
    private bool _jumpInput;
    private bool _attackInput;
    private bool _specialAttackInput;
    private bool _heal;
    public PlayerStats player;
    public PlayerHUD playerHud;


    public Vector2 MovementInput => _movementInput;
    public bool JumpInput => _jumpInput;
    public bool AttackInput => _attackInput;
    public bool SpecialAttackInput => _specialAttackInput;

    private void Update()
    {
        // Handle attack inputs
        _attackInput = Input.GetMouseButtonDown(1); // Right mouse button for attack
        _specialAttackInput = Input.GetKeyDown(KeyCode.Q);
        _heal = Input.GetKeyDown(KeyCode.F);
        player = GetComponent<PlayerStats>();
        playerHud = GetComponent<PlayerHUD>();

        // Handle point-and-click movement
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _targetPosition = hit.point;
                _isMoving = true;
            }
        }

        if(_heal){
            if(player.healingPotions==0){return;}
            player.healingPotions--;
            player.Heal();
            playerHud.UpdateHealthBar();
        }

        if (_isMoving)
        {
            Vector3 direction = (_targetPosition - transform.position);
            direction.y = 0; // Ignore vertical differences for movement
            if (direction.magnitude > 0.1f)
            {
                _movementInput = new Vector2(direction.normalized.x, direction.normalized.z);
            }
            else
            {
                _isMoving = false;
                _movementInput = Vector2.zero;
            }
        }
        else
        {
            _movementInput = Vector2.zero;
        }

        // Handle jump input
        _jumpInput = Input.GetButton("Jump");
    }
}
