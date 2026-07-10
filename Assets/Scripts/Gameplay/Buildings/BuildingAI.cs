using UnityEngine;

/// <summary>
/// Base AI controller for buildings (turrets, defensive structures).
/// Buildings do not move, but they can detect enemies, attack them,
/// launch projectiles, and take damage. This class is designed to be
/// inherited by specialized building types.
/// </summary>
public class BuildingAI : AIController
{
    [Header("Building Data")]
    public BuildingData data;

    /// <summary>
    /// Runtime copy of the building's stats.
    /// </summary>
    protected BuildingDataStruct buildingData;

    /// <summary>
    /// Current health of the building.
    /// </summary>
    protected float currentHealth;

    /// <summary>
    /// The current target enemy within range.
    /// </summary>
    protected GameObject currentTarget;

    /// <summary>
    /// Cached collider of the current target.
    /// </summary>
    protected Collider targetCollider;

    /// <summary>
    /// Location where projectiles are spawned.
    /// </summary>
    protected Transform projectileSpawnPoint;

    /// <summary>
    /// Prefab of the projectile to launch.
    /// </summary>
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;

    protected override void Start()
    {
        base.Start();

        buildingData = data.buildingData;
        currentHealth = buildingData.buildingHealth;

        // Buildings do not move
        if (navAgent != null)
            navAgent.enabled = false;

        // Optional projectile spawn point
        projectileSpawnPoint = transform.Find("ProjectileLaunchpoint");

        state = AIState.Defending;
    }

    protected override void Update()
    {
        attackTimer += Time.deltaTime;
        base.Update();
    }

    /// <summary>
    /// Determines the building's next state based on nearby enemies.
    /// </summary>
    protected override void Think()
    {
        if (state == AIState.Dying)
            return;

        // Defensive buildings do nothing except take damage
        if (buildingData.buildingType == EBuildingType.Defense)
        {
            state = AIState.Defending;
            return;
        }

        // Attack building logic
        GameObject newTarget = GetClosestEnemy();
        if (newTarget != null)
        {
            SetTarget(newTarget);

            float dist = Vector3.Distance(transform.position, currentTarget.transform.position);
            state = (dist <= buildingData.attackRange) ? AIState.Attack : AIState.Defending;
        }
        else
        {
            currentTarget = null;
            targetCollider = null;
            state = AIState.Defending;
        }
    }

    /// <summary>
    /// Executes the building's behavior based on its current state.
    /// </summary>
    protected override void Act()
    {
        switch (state)
        {
            case AIState.Attack:
                Attack(currentTarget);
                break;

            case AIState.Defending:
                // Idle behavior for buildings
                break;

            case AIState.Dying:
                break;
        }
    }

    /// <summary>
    /// Finds the closest enemy within attack range.
    /// </summary>
    protected virtual GameObject GetClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);

            if (dist < minDist && dist < buildingData.attackRange)
            {
                minDist = dist;
                closest = e;
            }
        }
        return closest;
    }

    /// <summary>
    /// Sets the current target and caches its collider.
    /// </summary>
    protected virtual void SetTarget(GameObject t)
    {
        currentTarget = t;
        targetCollider = t.GetComponent<Collider>();

        if (targetCollider == null)
            Debug.LogWarning($"[BuildingAI] No collider found for target {t.name}");
    }

    /// <summary>
    /// Performs an attack on the target. If a projectile prefab is assigned,
    /// launches a projectile instead of dealing instant damage.
    /// </summary>
    protected virtual void Attack(GameObject target)
    {
        if (target == null)
        {
            state = AIState.Defending;
            return;
        }

        if (attackTimer < buildingData.attackSpeed)
            return;

        attackTimer = 0f;

        // Projectile launching
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            LaunchProjectile(target);
            return;
        }

        // Instant damage fallback
        if (target.TryGetComponent<IDamageableInterface>(out var hit))
        {
            hit.TakeDamage(gameObject, buildingData.baseDamage, EDamageType.physical);
            Debug.Log($"[BuildingAI] Instant hit for {buildingData.baseDamage} damage.");
        }
    }

    /// <summary>
    /// Instantiates and launches a projectile toward the target.
    /// </summary>
    protected virtual void LaunchProjectile(GameObject target)
    {
        GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Ensure projectile faces the target
        proj.transform.LookAt(target.transform.position);

        // If projectile has a script, pass damage info
        if (proj.TryGetComponent<ProjectileBehavior>(out var projectile))
        {
            projectile.Initialize(target, buildingData.baseDamage, EDamageType.physical);
        }

        Debug.Log($"[BuildingAI] Launched projectile at {target.name} for {buildingData.baseDamage} damage.");
    }

    /// <summary>
    /// Applies damage to the building and destroys it if health reaches zero.
    /// </summary>
    public override void TakeDamage(GameObject attacker, float damage, EDamageType damageType)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            KillSelf();
        }
    }

    /// <summary>
    /// Handles building destruction and refunds partial cost.
    /// </summary>
    protected override void KillSelf()
    {
        PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        controller.Resource.SpendMoney((int)(-buildingData.buildingCost * 0.25f));

        Destroy(gameObject);
    }

    /// <summary>
    /// Applies stat upgrades to the building.
    /// </summary>
    public virtual void ApplyUpgrade(UpgradeObject upgrade)
    {
        foreach (var upgradeObj in upgrade.statUpgradesList)
        {
            switch (upgradeObj.upgradeType)
            {
                case EUpgradeType.buildingDamage:
                    buildingData.baseDamage += upgradeObj.upgradeValue;
                    break;

                case EUpgradeType.buildingHealth:
                    buildingData.buildingHealth += upgradeObj.upgradeValue;
                    currentHealth += upgradeObj.upgradeValue;
                    break;
            }
        }
    }
}
