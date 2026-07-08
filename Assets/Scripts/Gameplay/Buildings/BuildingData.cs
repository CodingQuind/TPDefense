using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Scriptable Objects/BuildingData")]
[System.Serializable]
public class BuildingData : ScriptableObject
{
    public BuildingDataStruct buildingData;
}
[System.Serializable]
public struct BuildingDataStruct
{
    public GameObject buildingPrefab;
    public GameObject ghostPrefab;
    public EBuildingType buildingType;
    public Sprite buildingIcon;
    public string buildingName;
    public int buildingCost;
    public float buildingHealth;
    public float buildingEnergy;
    public int buildingLevel;
    public int buildingXp;
    public int baseDamage;
    public float attackSpeed;
    public float attackRange;

    public BuildingDataStruct(GameObject model, GameObject ghostModel, EBuildingType buildingType, Sprite icon, string name, int cost, float health, float energy, int level, int xp, int baseDamage, float attackSpeed, float attackRange)
    {
        buildingPrefab = model;
        ghostPrefab = ghostModel;
        this.buildingType = buildingType;
        buildingIcon = icon;
        buildingName = name;
        buildingCost = cost;
        buildingHealth = health;
        buildingEnergy = energy;
        buildingLevel = level;
        buildingXp = xp;
        this.baseDamage = baseDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }
}