using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesTable", menuName = "Scriptable Objects/UpgradesTable")]
public class UpgradesTable : ScriptableObject
{
    public UpgradeObject[] upgrades;
    public string upgradeTableName = "default";
    public UpgradeObject GetUpgradeByName(string name)
    {
        foreach (UpgradeObject upgrade in upgrades)
        {
            if (upgrade.upgradeName == name)
            {
                return upgrade;
            }
        }
        throw new System.Exception($"Upgrade with name {name} not found in table {upgradeTableName}");
    }

    public UpgradeObject GetUpgradeByIndex(int index)
    {
        if (index < 0 || index >= upgrades.Length)
        {
            throw new System.Exception("Index out of range for upgrades table " + upgradeTableName);
        }
        return upgrades[index];
    }

    public string GetName() { return upgradeTableName; }
}
