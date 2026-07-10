using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Stat system for the player. Handles leveling, XP, class attributes,
/// upgrades, and respawn logic.
/// </summary>
public class PlayerStatSystem : BaseStatSystem
{
    [Header("Player Attributes")]
    [SerializeField] private PlayerController playerController;

    private int level = StatSystemSettings.defaultLevel;
    private int currentXp = StatSystemSettings.defaultXp;
    private int requiredXp;

    private int baseStrength = 5, baseAgility = 4, baseIntelligence = 3, baseConstitution = 2;
    private int speedModifier = 1, jumpModifier = 1;

    private float basePhysicalDmg = 10, baseMagicDmg = 10;
    public EClasses Class { get; private set; } = EClasses.Warrior;

    private List<UpgradeObject> appliedUpgrades = new();

    public override void InitializeStats()
    {
        MaxHealth = StatSystemSettings.defaultHealth;
        MaxEnergy = StatSystemSettings.defaultEnergy;

        base.InitializeStats();

        requiredXp = CalculateRequiredXp(level);
    }

    private int CalculateRequiredXp(int level)
    {
        return (level * level) * 100;
    }

    public void GrantXp(int xp)
    {
        currentXp += xp;
        if (currentXp >= requiredXp)
            LevelUp();
    }

    private void LevelUp()
    {
        level++;
        currentXp = 0;
        requiredXp = CalculateRequiredXp(level);

        float oldHealth = MaxHealth;
        UpdateMaxHealth();
        Heal(MaxHealth - oldHealth);

        float oldEnergy = MaxEnergy;
        UpdateMaxEnergy();
        CurrentEnergy += (MaxEnergy - oldEnergy);
    }

    private void UpdateMaxHealth()
    {
        MaxHealth = (baseConstitution * level) * 1000;
    }

    private void UpdateMaxEnergy()
    {
        MaxEnergy = (baseIntelligence * level) * 100;
    }

    public override float GetPhysicalDamage()
    {
        return Class switch
        {
            EClasses.Warrior => (int)(basePhysicalDmg + (baseStrength * 1.4f)),
            EClasses.Assassin => (int)(basePhysicalDmg + (baseAgility * 1.2f)),
            EClasses.Mage => (int)(basePhysicalDmg + (baseStrength * 1.4f)),
            EClasses.Builder => (int)(basePhysicalDmg + (baseStrength * 1.4f)),
            _ => basePhysicalDmg
        };
    }

    public float GetMagicDamage()
    {
        return baseMagicDmg + (baseIntelligence * 2);
    }

    public void ClassUpdate(EClasses newClass)
    {
        switch (newClass)
        {
            case EClasses.Warrior:
                baseStrength = 12; baseAgility = 5; baseIntelligence = 2; baseConstitution = 8;
                break;
            case EClasses.Assassin:
                baseStrength = 7; baseAgility = 12; baseIntelligence = 3; baseConstitution = 5;
                break;
            case EClasses.Mage:
                baseStrength = 3; baseAgility = 4; baseIntelligence = 14; baseConstitution = 4;
                break;
            case EClasses.Builder:
                baseStrength = 4; baseAgility = 4; baseIntelligence = 4; baseConstitution = 4;
                break;
        }

        Class = newClass;
        UpdateMaxHealth();
        UpdateMaxEnergy();
    }

    public void AddUpgrade(UpgradeObject upgrade)
    {
        appliedUpgrades.Add(upgrade);

        foreach (var statUpgrade in upgrade.statUpgradesList)
        {
            switch (statUpgrade.upgradeType)
            {
                case EUpgradeType.strength:
                    baseStrength += statUpgrade.upgradeValue;
                    break;
                case EUpgradeType.agility:
                    baseAgility += statUpgrade.upgradeValue;
                    break;
                case EUpgradeType.intelligence:
                    baseIntelligence += statUpgrade.upgradeValue;
                    UpdateMaxEnergy();
                    break;
                case EUpgradeType.constitution:
                    baseConstitution += statUpgrade.upgradeValue;
                    UpdateMaxHealth();
                    break;
                case EUpgradeType.speed:
                    speedModifier += statUpgrade.upgradeValue;
                    break;
                case EUpgradeType.jumpHeight:
                    jumpModifier += statUpgrade.upgradeValue;
                    break;
            }
        }
    }

    public void Respawn()
    {
        CurrentHealth = MaxHealth;
        CurrentEnergy = MaxEnergy;
        level = 1;
        currentXp = 0;
        requiredXp = CalculateRequiredXp(1);
    }
}
