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
        throw new System.NotImplementedException();
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
}

