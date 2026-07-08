using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static EnemyBehavior;

public class BuildingBehavior : MonoBehaviour, IDamageableInterface
{
    public BuildingData data;
    private BuildingDataStruct buildingData;
    private float currentHealth;
    private float attackTimer = 0f;

    private GameObject[] enemies;
    private GameObject currentTarget;
    private Transform projSpawnLoc;
    private Collider targetCollider;
    private AIState state = AIState.Defending;

    public void DamageTarget(GameObject target, float damage, EDamageType damageType)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(GameObject attacker, float damage, EDamageType damageType)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) 
        {
            DestroyBuilding();
        }
    }

    private void DestroyBuilding()
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.SpendMoney((int)(-buildingData.buildingCost *.25));
        Destroy(gameObject);
    }
    void Start()
    {
        buildingData = data.buildingData;
        currentHealth = buildingData.buildingHealth;
        projSpawnLoc = transform.Find("ProjectileLaunchpoint").gameObject.transform;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        attackTimer += Time.deltaTime;
        GameObject newTarget = GetClosestEnemy();
        if (newTarget != null)
        {
            SetTarget(newTarget);
        }
        else
        {
            currentTarget = null;
            targetCollider = null;
        }

        if (currentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (dist <= buildingData.attackRange)
            {
                state = AIState.Attack;
            }
        }

        switch (state)
        {

            case AIState.Attack:
                if (currentTarget == null)
                {
                    state = AIState.Defending;
                    break;
                }

                float attackDist = Vector3.Distance(transform.position, currentTarget.transform.position);

                if (attackDist > buildingData.attackRange)
                {
                    state = AIState.Defending;
                }
                else
                {
                    Attack(currentTarget);
                }
                break;
            case AIState.Dying:
                break;
            case AIState.Defending:
                break;
        }
    }

    private GameObject GetClosestEnemy()
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

    void Attack(GameObject target)
    {
        var hit = target.GetComponent<IDamageableInterface>();
        if (attackTimer >= buildingData.attackSpeed)
        {
            if (hit != null)
            {
                hit.TakeDamage(this.gameObject, buildingData.baseDamage, EDamageType.physical);
                Debug.Log("[BuildingBehavior] DEBUG: hitting target for " + buildingData.baseDamage + " damage.");
                attackTimer = 0f;
            }
        }

    }


    void SetTarget(GameObject t)
    {
        currentTarget = t;

        // Find the collider anywhere above this object
        targetCollider = t.GetComponent<Collider>();

        // Optional safety check
        if (targetCollider == null)
            Debug.LogWarning($"No collider found for target {t.name}");
    }

    public void ApplyUpgrade(UpgradeObject upgrade)
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
                default:
                    break;
            }
        }
    }
}

