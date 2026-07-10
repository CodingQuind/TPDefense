using UnityEngine;

/// <summary>
/// Stat system for buildings. Handles building health, damage, and upgrades.
/// </summary>
public class BuildingStatSystem : BaseStatSystem
{
    public BuildingDataStruct buildingData;

    public override void InitializeStats()
    {
        MaxHealth = buildingData.buildingHealth;
        MaxEnergy = 0;

        base.InitializeStats();
    }

    public float GetDamage()
    {
        return buildingData.baseDamage;
    }

    public void ApplyUpgrade(UpgradeObject upgrade)
    {
        foreach (var statUpgrade in upgrade.statUpgradesList)
        {
            switch (statUpgrade.upgradeType)
            {
                case EUpgradeType.buildingDamage:
                    buildingData.baseDamage += statUpgrade.upgradeValue;
                    break;

                case EUpgradeType.buildingHealth:
                    buildingData.buildingHealth += statUpgrade.upgradeValue;
                    MaxHealth = buildingData.buildingHealth;
                    CurrentHealth += statUpgrade.upgradeValue;
                    break;
            }
        }
    }
}
