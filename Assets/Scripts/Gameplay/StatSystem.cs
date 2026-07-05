using UnityEngine;

public class StatSystem : MonoBehaviour
{
    [Header("Stat System Defaults")]
    private float maxHealth = StatSystemSettings.defaultHealth;
    private float maxEnergy = StatSystemSettings.defaultEnergy;
    private int level = StatSystemSettings.defaultLevel, currentXp = StatSystemSettings.defaultXp;
    private float currentEnergy, currentHealth;
    private int requiredXp;

    // NOTE: Base attributes will be removed and be based on class instead. This is just a temporary solution to get the system working.
    [Header("Attributes")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private int baseStrength = 5, baseAgility = 4, baseIntelligence = 3, baseConstitution = 2;

    private int basePhysicalDmg = 10, baseMagicDmg = 10;
    private EClasses currentClass = EClasses.Warrior;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeStats()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        requiredXp = CalculateRequiredXp(level);
    }

    // Basic formula for now. just 100 * level
    private int CalculateRequiredXp(int level) { return (level^2) * 100; }

    private void GrantXp(int xp)
    {
        currentXp += xp;
        if (currentXp >= requiredXp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // Increase level, update stats, and reset current XP
        level += 1;
        currentXp = 0;
        requiredXp = CalculateRequiredXp(level);

        var oldHealth = maxHealth;
        UpdateMaxHealth();
        UpdateHealth(maxHealth - oldHealth);

        var oldEnergy = maxEnergy;
        UpdateMaxEnergy();
        UpdateEnergy(maxEnergy - oldEnergy);
    }

    private void UpdateHealth(float healthToAdd)
    {
        currentHealth = Mathf.Clamp(currentHealth + healthToAdd, 0, maxHealth);
    }

    private void UpdateMaxHealth()
    {
        float newMaxHealth = (baseConstitution * level) * 100;
        maxHealth = newMaxHealth;
    }

    private void UpdateEnergy(float energyToAdd)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + energyToAdd, 0, maxEnergy);
    }

    private void UpdateMaxEnergy()
    {
        float newMaxEnergy = (baseIntelligence * level) * 100;
        maxEnergy = newMaxEnergy;
    }

    public void Damage(float damageAmount)
    {
        UpdateHealth(-damageAmount);
    }

    public void Heal(float healAmount)
    {
        UpdateHealth(healAmount);
    }

    public int GetStrength() { return baseStrength; }
    public int GetAgility() { return baseAgility; }
    public int GetIntelligence() { return baseIntelligence; }
    public int GetConstitution() { return baseConstitution; }
    public int GetPhysicalDamage() 
    {
        switch (currentClass)
        {
            case EClasses.Warrior:
            case EClasses.Mage:
            case EClasses.Builder:
                return basePhysicalDmg + (baseStrength * 2);
            case EClasses.Assassin:
                return basePhysicalDmg + (baseAgility * 2);
            default:
                return basePhysicalDmg;
        }
    }
    public int GetMagicDamage() { return baseMagicDmg + (baseIntelligence * 2); }
    public void ClassUpdate(EClasses newClass)
    {
        // Hardcoded values, later could be in a datatable
        switch (newClass)
        {
            case EClasses.Warrior:
                baseStrength = 12;
                baseAgility = 5;
                baseIntelligence = 2;
                baseConstitution = 8;
                break;
            case EClasses.Assassin:
                baseStrength = 7;
                baseAgility = 12;
                baseIntelligence = 3;
                baseConstitution = 5;
                break;
            case EClasses.Mage:
                baseStrength = 3;
                baseAgility = 4;
                baseIntelligence = 14;
                baseConstitution = 4;
                break;
            case EClasses.Builder:
                baseStrength = 4;
                baseAgility = 4;
                baseIntelligence = 4;
                baseConstitution = 4;
                break;   
        }
        currentClass = newClass;
    }
}
