using UnityEngine;

public class StatSystem : MonoBehaviour
{
    [Header("Stat System Defaults")]
    private float currentHealth, maxHealth = StatSystemSettings.defaultHealth;
    private float currentEnergy, maxEnergy = StatSystemSettings.defaultEnergy;
    private int level = StatSystemSettings.defaultLevel, currentXp = StatSystemSettings.defaultXp, requiredXp;

    [Header("Attributes")]
    private int baseStrength = 0, baseAgility = 0, baseIntelligence = 0, baseConstitution = 0;
    
    private int basePhysicalDmg = 10, baseMagicDmg = 10;

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
    private int CalculateRequiredXp(int level) { return level * 100;  }

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
    }

    private void UpdateHealth(float healthToAdd)
    {
        currentHealth = Mathf.Clamp(currentHealth + healthToAdd, 0, maxHealth);
    }

    private void UpdateMaxHealth(float newMaxHealth)
    {
        float diff = newMaxHealth - maxHealth;
        maxHealth = newMaxHealth;
        UpdateHealth(diff);
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
    public int GetPhysicalDamage() { return basePhysicalDmg; }
    public int GetMagicDamage() { return baseMagicDmg; }


}
