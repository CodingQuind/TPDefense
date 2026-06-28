using UnityEngine;

public class StatSystem : MonoBehaviour
{
    [Header("Stat System Defaults")]
    private float currentHealth, maxHealth;
    private float currentEnergy, maxEnergy;
    private int level, currentXp, requiredXp;

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
        maxHealth = StatSystemSettings.defaultHealth;
        currentHealth = maxHealth;
        maxEnergy = StatSystemSettings.defaultEnergy;
        currentEnergy = maxEnergy;
        level = StatSystemSettings.defaultLevel;
        currentXp = StatSystemSettings.defaultXp;
        requiredXp = CalculateRequiredXp(level);
    }

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

}
