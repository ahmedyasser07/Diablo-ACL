using UnityEngine;
using NaughtyCharacter;

namespace Retro.ThirdPersonCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Animator))]
    public class Combat : MonoBehaviour
    {
        private const string attackTriggerName = "Attack";
        private const string specialAttackTriggerName = "Ability";

        private Animator _animator;
        private PlayerInput _playerInput;

        public bool AttackInProgress { get; private set; } = false;

        [Header("Attack Settings")]
        public int damage = 10; // Damage per attack
        public float attackRange = 1.5f; // Range of the attack
        public Transform attackPoint; // Point from where the attack originates
        public LayerMask enemyLayer; // Layer mask to identify enemies

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();

            if (attackPoint == null)
            {
                attackPoint = this.transform;
                Debug.LogWarning("No attackPoint specified. Defaulting to player's transform.");
            }

            if (enemyLayer == 0)
            {
                Debug.LogWarning("Enemy LayerMask not set in Combat script. Please set the layer.");
            }

            Debug.Log("Combat script initialized.");
        }

        private void Update()
        {
            // If Q (AttackInput) is pressed and no attack is in progress, start Attack
            if (_playerInput.AttackInput && !AttackInProgress)
            {
                Debug.Log("Player pressed attack (Q).");
                Attack();
            }
            // If W (SpecialAttackInput) is pressed and no attack is in progress, start Special Attack
            else if (_playerInput.SpecialAttackInput && !AttackInProgress)
            {
                Debug.Log("Player pressed special attack (W).");
                SpecialAttack();
            }
        }

        private void SetAttackStart()
        {
            Debug.Log("Animation event: Attack start.");
            AttackInProgress = true;
        }

        private void SetAttackEnd()
        {
            Debug.Log("Animation event: Attack end.");
            AttackInProgress = false;
        }

        private void Attack()
        {
            Debug.Log("Triggering attack animation.");
            _animator.SetTrigger(attackTriggerName);
            
            // If you want to deal damage immediately when pressing Q (not waiting for animation event),
            // you could call PerformAttackHit() here.
             PerformAttackHit();
        }

       private void SpecialAttack()
{
    Debug.Log("Triggering special attack animation.");
    _animator.SetTrigger(specialAttackTriggerName);

    // Immediate damage for special attack
    PerformAttackHit();
}

// This method should be called by an Animation Event at the moment the attack "connects".
// Set an animation event in your attack/special attack animations to call this method.
private void PerformAttackHit()
{
    Debug.Log("Animation event: Performing attack hit.");
    Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

    foreach (Collider enemy in hitEnemies)
    {
        // Directly check for a "TakeDamage" method on the enemy
        var enemyStats = enemy.GetComponent<EnemyAi>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(damage);
        }
        // If not EnemyAi, try MinionAi
        var minionStats = enemy.GetComponent<MinionAI>();
        if (minionStats != null)
        {
            minionStats.TakeDamage(damage);
        }
    }
}

// Optional: visualize the attack range in editor
private void OnDrawGizmosSelected()
{
    if (attackPoint == null) return;
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
}

}
}