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
    public string buildingName;
    public int buildingCost;
    public float buildingHealth;
    public float buildingEnergy;
    public int buildingLevel;
    public int buildingXp;

    public BuildingDataStruct(string name, int cost, float health, float energy, int level, int xp)
    {
        buildingName = name;
        buildingCost = cost;
        buildingHealth = health;
        buildingEnergy = energy;
        buildingLevel = level;
        buildingXp = xp;
    }
}