using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesTable", menuName = "Scriptable Objects/UpgradesTable")]
public class UpgradesTable : ScriptableObject
{
    public Upgrade[] upgrades;
    public string upgradeTableName = "default";
    public Upgrade GetUpgradeByName(string name)
    {
        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.name == name)
            {
                return upgrade;
            }
        }
        return null;
    }

    public Upgrade GetUpgradeByIndex(int index)
    {
        if (index < 0 || index >= upgrades.Length)
        {
            return null;
        }
        return upgrades[index];
    }

    public string GetName() { return upgradeTableName; }
}
