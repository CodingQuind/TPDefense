using UnityEngine;

[System.Serializable]
public struct UpgradeObject
{
    public string upgradeName;
    public int cost;
    public UpgradeStruct[] statUpgradesList;
    public Sprite icon;

    public UpgradeObject(string name, int cost, UpgradeStruct[] statUpgradesList, Sprite icon)
    {
        this.upgradeName = name;
        this.cost = cost;
        this.statUpgradesList = statUpgradesList;
        this.icon = icon;
    }
}

