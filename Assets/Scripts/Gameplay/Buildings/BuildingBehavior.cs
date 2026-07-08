using UnityEngine;

public class BuildingBehavior : MonoBehaviour, IDamageableInterface
{
    public BuildingData data;
    private BuildingDataStruct buildingData;
    private float currentHealth;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildingData = data.buildingData;
        currentHealth = buildingData.buildingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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

