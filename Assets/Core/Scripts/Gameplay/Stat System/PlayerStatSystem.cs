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

    public  int Level { get; private set; }
    public int XP { get; private set; }
    private int requiredXp;

    public int Strength { get; private set; } = 5;
    public int Agility { get; private set; } = 4;
    public int Intelligence { get; private set; } = 3; 
    public int Constitution { get; private set; } = 2;
    private int SpeedModifier = 1, jumpModifier = 1;

    private float basePhysicalDmg = 10, baseMagicDmg = 10;
    public EClasses Class { get; private set; } = EClasses.Warrior;

    private List<UpgradeObject> appliedUpgrades = new();

    public override void InitializeStats()
    {
        MaxHealth = GameSettings.Instance.StatSystemSettings.defaultHealth;
        MaxEnergy = GameSettings.Instance.StatSystemSettings.defaultEnergy;
        Level = GameSettings.Instance.StatSystemSettings.defaultLevel;
        XP = GameSettings.Instance.StatSystemSettings.defaultXp;

        base.InitializeStats();

        requiredXp = CalculateRequiredXp(Level);
    }

    private int CalculateRequiredXp(int level)
    {
        return (level * level) * 100;
    }

    public void GrantXp(int xp)
    {
        XP += xp;
        if (XP >= requiredXp)
            LevelUp();
    }

    private void LevelUp()
    {
        Level++;
        XP = 0;
        requiredXp = CalculateRequiredXp(Level);

        float oldHealth = MaxHealth;
        UpdateMaxHealth();
        Heal(MaxHealth - oldHealth);

        float oldEnergy = MaxEnergy;
        UpdateMaxEnergy();
        CurrentEnergy += (MaxEnergy - oldEnergy);
    }

    private void UpdateMaxHealth()
    {
        MaxHealth = (Constitution * Level) * 1000;
    }

    private void UpdateMaxEnergy()
    {
        MaxEnergy = (Intelligence * Level) * 100;
    }

    public override float GetPhysicalDamage()
    {
        return Class switch
        {
            EClasses.Warrior => (int)(basePhysicalDmg + (Strength * 1.4f)),
            EClasses.Assassin => (int)(basePhysicalDmg + (Agility * 1.2f)),
            EClasses.Mage => (int)(basePhysicalDmg + (Strength * 1.4f)),
            EClasses.Builder => (int)(basePhysicalDmg + (Strength * 1.4f)),
            _ => basePhysicalDmg
        };
    }

    public float GetMagicDamage()
    {
        return baseMagicDmg + (Intelligence * 2);
    }

    public void ClassUpdate(EClasses newClass)
    {
        switch (newClass)
        {
            case EClasses.Warrior:
                Strength = 12; Agility = 5; Intelligence = 2; Constitution = 8;
                break;
            case EClasses.Assassin:
                Strength = 7; Agility = 12; Intelligence = 3; Constitution = 5;
                break;
            case EClasses.Mage:
                Strength = 3; Agility = 4; Intelligence = 14; Constitution = 4;
                break;
            case EClasses.Builder:
                Strength = 4; Agility = 4; Intelligence = 4; Constitution = 4;
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
                    Strength += statUpgrade.upgradeValue;
                    break;
                case EUpgradeType.agility:
                    Agility += statUpgrade.upgradeValue;
                    break;
                case EUpgradeType.intelligence:
                    Intelligence += statUpgrade.upgradeValue;
                    UpdateMaxEnergy();
                    break;
                case EUpgradeType.constitution:
                    Constitution += statUpgrade.upgradeValue;
                    UpdateMaxHealth();
                    break;
                case EUpgradeType.speed:
                    SpeedModifier += statUpgrade.upgradeValue;
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
        Level = 1;
        XP = 0;
        requiredXp = CalculateRequiredXp(1);
    }
}
