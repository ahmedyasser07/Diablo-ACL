using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Sorcerer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Movement Settings")]
    public float walkSpeed = 3.5f;        // Normal walking speed
    public float runSpeed = 6f;          // Running speed
    public float stoppingThreshold = 0.1f; // Velocity threshold to consider stopped
    public float rotationSpeed = 10f;    // Speed of character rotation

    private bool isWalking = false;      // Tracks if the character is currently walking

    [Header("Fireball Settings")]
    public GameObject fireballPrefab;    // Fireball prefab
    public Transform firePoint;          // Fire point (where the fireball spawns)
    public float fireballSpeed = 10f;    // Speed of the fireball
    public float fireCooldown = 1f;     // Cooldown time for fireball casting

    private float fireCooldownTimer = 0f;

    [Header("Leveling System")]
    public int level = 1;           // Current level
    public int currentXP = 0;       // Current XP
    public int maxXP = 100;         // Max XP for the current level
    public int abilityPoints = 0;   // Ability points available
    public int maxHealth = 100;     // Max health
    public int currentHealth = 100; // Current health

    [Header("Abilities Unlocking")]
    public bool teleportUnlocked = false; // Is Teleport unlocked?
    public bool cloneUnlocked = false;    // Is Clone unlocked?
    public bool infernoUnlocked = false; // Is Inferno unlocked?

    [Header("Teleport Ability")]
    public float teleportCooldown = 10f; // Cooldown time
    private float teleportCooldownTimer = 0f; // Tracks cooldown status
    private bool teleportActivated = false;  // Tracks if teleport mode is active

    [Header("Clone Ability")]
    public GameObject clonePrefab;        // Prefab for the clone
    public float cloneCooldown = 10f;     // Cooldown for clone ability
    private float cloneCooldownTimer = 0f; // Tracks clone cooldown status
    public float cloneDuration = 5f;      // Duration the clone lasts
    public float cloneExplosionDamage = 10f; // Damage dealt by clone explosion
    private bool cloneActivated = false;  // Tracks if clone mode is active

    [Header("Inferno Ability")]
public GameObject infernoPrefab;      // Prefab for the Inferno ring
public float infernoCooldown = 15f;   // Cooldown for Inferno ability
private float infernoCooldownTimer = 0f; // Tracks cooldown status
public float infernoDuration = 5f;    // Duration the Inferno lasts
public int infernoInitialDamage = 10; // Initial damage
public int infernoDamagePerSecond = 2; // Damage per second
public float infernoRadius = 5f;      // Radius of the Inferno ring
private bool infernoActivated = false;  // Tracks if inferno mode is active

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null) Debug.LogError("NavMeshAgent component is missing!");
        if (animator == null) Debug.LogError("Animator component is missing!");

        agent.updateRotation = false; // Disable automatic NavMeshAgent rotation
        animator.applyRootMotion = false;

        // Set initial agent speed to walking speed
        agent.speed = walkSpeed;
        teleportUnlocked = true;
        cloneUnlocked = true; 
    }

void Update()
{
    HandleMovement();
    HandleAnimation();
    HandleFireballOrTeleport();
    HandleAbilities();

    if (Input.GetKeyDown(KeyCode.X)) // Simulate XP gain
    {
        GainXP(50);
        Debug.Log($"Gained XP! Current XP: {currentXP}/{maxXP}");
    }

    // Reduce timers
    if (teleportCooldownTimer > 0f) teleportCooldownTimer -= Time.deltaTime;
    if (cloneCooldownTimer > 0f) cloneCooldownTimer -= Time.deltaTime;
    if (infernoCooldownTimer > 0f) infernoCooldownTimer -= Time.deltaTime;

    // Activate Inferno ability
    if (Input.GetKeyDown(KeyCode.E))
    {
        if (infernoCooldownTimer <= 0f) // Only activate if cooldown is 0
        {
            infernoActivated = true;
            Debug.Log("Inferno ability activated. Right-click to place the flames.");
        }
        else
        {
            Debug.Log($"Inferno ability is on cooldown. Time remaining: {Mathf.Ceil(infernoCooldownTimer)} seconds.");
        }
    }

    // Cast Inferno with Right-Click
    if (Input.GetMouseButtonDown(1) && infernoActivated)
    {
        CastInferno();
        infernoCooldownTimer = infernoCooldown; // Reset cooldown
        infernoActivated = false;              // Deactivate ability
    }
}


    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0))  // Left mouse click to move
        {
            MoveToClickPosition();
        }

        // Adjust speed based on input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            agent.speed = runSpeed;  // Running when Left Shift is held
        }
        else
        {
            agent.speed = walkSpeed;  // Default to walking
        }

        // Rotate to face the direction of movement
        RotateTowardsMovementDirection();
    }

    private void MoveToClickPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Set destination for NavMeshAgent
            agent.SetDestination(hit.point);

            // Start walking animation if not already walking
            if (!isWalking)
            {
                animator.SetTrigger("StartWalking");
                isWalking = true;
            }

            // Ensure agent is not stopped
            agent.isStopped = false;
        }
    }

private void HandleAnimation()
{
    // Check if the agent is active and on a NavMesh
    if (!agent.isActiveAndEnabled || !agent.isOnNavMesh)
    {
        Debug.LogWarning("Agent is not active or not on a NavMesh. Skipping animation handling.");
        return; // Exit the method to avoid errors
    }

    if (agent.pathPending) return;

    // Check if the agent is near the target or has stopped moving
    if (agent.remainingDistance <= agent.stoppingDistance && agent.velocity.magnitude <= stoppingThreshold)
    {
        if (isWalking)
        {
            // Trigger idle animation and stop walking
            animator.SetTrigger("StopWalking");
            isWalking = false;

            // Stop NavMeshAgent completely
            agent.isStopped = true;
            agent.ResetPath();
        }
    }
    else if (!isWalking)
    {
        // If the agent is moving but walking animation is not active, start it
        animator.SetTrigger("StartWalking");
        isWalking = true;
    }
}


    private void RotateTowardsMovementDirection()
    {
        // Get the desired velocity of the NavMeshAgent
        Vector3 direction = agent.desiredVelocity;

        // Only rotate if there is a significant direction to face
        if (direction.sqrMagnitude > 0.01f)
        {
            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly interpolate to the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    private void CastInferno()
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out RaycastHit hit))
    {
        // Trigger the Inferno animation
        animator.SetTrigger("InfernoCast");

        // Instantiate the Inferno prefab at the selected position
        GameObject inferno = Instantiate(infernoPrefab, hit.point, Quaternion.identity);
        Debug.Log($"Inferno cast at {hit.point}");

        // Start Inferno behavior
        InfernoBehavior infernoScript = inferno.GetComponent<InfernoBehavior>();
        if (infernoScript != null)
        {
            infernoScript.Initialize(infernoDuration, infernoInitialDamage, infernoDamagePerSecond, infernoRadius);
        }
    }
    else
    {
        Debug.Log("Invalid target position for Inferno.");
    }
}


 private void HandleAbilities()
    {
        if (Input.GetKeyDown(KeyCode.Q) && cloneUnlocked && cloneCooldownTimer <= 0f)
        {
            cloneActivated = true;
            Debug.Log("Clone mode activated. Right-click to place the clone.");
        }
        else if (Input.GetKeyDown(KeyCode.Q) && cloneCooldownTimer > 0f)
        {
            Debug.Log($"Clone ability is on cooldown. Time remaining: {Mathf.Ceil(cloneCooldownTimer)} seconds.");
        }

        if (Input.GetMouseButtonDown(1) && cloneActivated)
        {
            SpawnClone();
            cloneActivated = false; 
        }
    } 
    private void SpawnClone()
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out RaycastHit hit))
    {
        if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
        {
            GameObject clone = Instantiate(clonePrefab, navHit.position, Quaternion.identity);
            Debug.Log($"Clone spawned at {navHit.position}");

            // Ensure the clone is immobile
            DisableCloneMovement(clone);

            StartCoroutine(CloneBehavior(clone));
            cloneActivated = false; // Deactivate clone mode after spawning
            cloneCooldownTimer = cloneCooldown; // Start cooldown timer
        }
        else
        {
            Debug.Log("Cannot place the clone here!");
        }
    }
}

 private void DisableCloneMovement(GameObject clone)
{
    // Disable NavMeshAgent if present
    NavMeshAgent agent = clone.GetComponent<NavMeshAgent>();
    if (agent != null)
    {
        agent.enabled = false;
    }

    // Disable Rigidbody movement
    Rigidbody rb = clone.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true; // Prevent physics-based movement
    }

    // Set clone Animator to idle and disable root motion
    Animator animator = clone.GetComponent<Animator>();
    if (animator != null)
    {
        animator.applyRootMotion = false; // Disable root motion
        animator.SetTrigger("Idle");     // Ensure the clone plays an idle animation
    }
}


   [SerializeField] private GameObject explosionEffect; // Drag your explosion prefab here
[SerializeField] private float explosionRadius = 5f; // Radius of the explosion
[SerializeField] private LayerMask enemyLayer;       // Layer for enemies to detect

private IEnumerator CloneBehavior(GameObject clone)
{
    bool hasExploded = false; // Ensure the explosion happens only once

    // Wait for the clone's duration
    yield return new WaitForSeconds(cloneDuration);

    // Trigger explosion if it hasn't already happened
    if (!hasExploded)
    {
        hasExploded = true;

        // Instantiate explosion effect at the clone's position
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, clone.transform.position, Quaternion.identity);
            Debug.Log("Explosion effect instantiated.");
        }

        // Deal damage to nearby enemies
        Collider[] hitColliders = Physics.OverlapSphere(clone.transform.position, explosionRadius, enemyLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            // Check if the collider has an enemy script or component
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Mathf.RoundToInt(cloneExplosionDamage));
                Debug.Log($"Enemy {enemy.name} takes {Mathf.RoundToInt(cloneExplosionDamage)} damage from clone explosion.");
            }
        }

        // Destroy the clone after the explosion
        Destroy(clone);
        Debug.Log("Clone exploded and was destroyed.");
    }
}


    private void HandleFireballOrTeleport()
{
    // Activate teleport mode only if teleport is unlocked, not on cooldown, and 'W' is pressed
    if (Input.GetKeyDown(KeyCode.W) && teleportUnlocked && teleportCooldownTimer <= 0f)
    {
        teleportActivated = true;
        Debug.Log("Teleport mode activated. Right-click to teleport.");
    }
    else if (Input.GetKeyDown(KeyCode.W) && teleportCooldownTimer > 0f)
    {
        Debug.Log($"Teleport is on cooldown. Time remaining: {Mathf.Ceil(teleportCooldownTimer)} seconds.");
    }

    // Handle right-click input for teleport or fireball
    if (Input.GetMouseButtonDown(1))
    {
        if(infernoActivated){
           CastInferno();
            infernoActivated = false; // Deactivate Inferno mode after casting
            return;
        }
        if (teleportActivated)
        {
            Teleport();
            teleportActivated = false; // Deactivate teleport mode after use
            Debug.Log("Teleport completed. Exiting teleport mode.");
            return;
        }
        if(cloneActivated){
            HandleAbilities();
            return;
        }
        if (fireCooldownTimer <= 0f) // Fireball logic
        {
            animator.SetTrigger("Throw");  // Activate the throwing animation
            fireCooldownTimer = fireCooldown;  // Reset cooldown
        }
         
    }

    // Reduce cooldown timers
    if (fireCooldownTimer > 0f)
    {
        fireCooldownTimer -= Time.deltaTime;
    }
}


    public void ShootFireball()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // Spawn the fireball
                GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

                // Set the fireball's target
                Fireball fireballScript = fireball.GetComponent<Fireball>();
                if (fireballScript != null)
                {
                    fireballScript.SetTarget(hit.collider.transform);
                }

                Debug.Log("Fireball cast toward: " + hit.collider.name);
            }
        }
    }

    public void GainXP(int amount)
    {
        currentXP += amount;

        // Handle Level Up
        while (currentXP >= maxXP && level < 4) // Max level is 4
        {
            currentXP -= maxXP; // Overflow XP
            level++;
            abilityPoints++;
            maxHealth += 100;
            currentHealth = maxHealth; // Refill health
            maxXP = 100 * level; // Update max XP for the new level

            Debug.Log($"Level Up! New Level: {level}, Ability Points: {abilityPoints}");
        }
    }

    public void Teleport()
    {
        // Check if Teleport is unlocked and cooldown is inactive
        if (!teleportUnlocked || teleportCooldownTimer > 0f) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if the hit point is walkable
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                // Perform teleport
                transform.position = navHit.position;

                // Start cooldown
                teleportCooldownTimer = teleportCooldown;

                Debug.Log($"Teleported to {navHit.position}");
            }
            else
            {
                Debug.Log("Cannot teleport to this position!");
            }
        }
    }
}
