using UnityEngine;

/// <summary>
/// Base stat system for all entities (player, enemies, buildings).
/// Handles health, energy, damage, healing, and initialization.
/// </summary>
public abstract class BaseStatSystem : MonoBehaviour
{
    /// <summary>Maximum health of the entity.</summary>
    public float MaxHealth { get; protected set; }

    /// <summary>Current health of the entity.</summary>
    public float CurrentHealth { get; protected set; }

    /// <summary>Maximum energy/mana/stamina.</summary>
    public float MaxEnergy { get; protected set; }

    /// <summary>Current energy.</summary>
    public float CurrentEnergy { get; protected set; }

    /// <summary>
    /// Initializes health and energy values.
    /// </summary>
    public virtual void InitializeStats()
    {
        CurrentHealth = MaxHealth;
        CurrentEnergy = MaxEnergy;
    }

    /// <summary>
    /// Applies damage and returns true if the entity dies.
    /// </summary>
    public virtual bool Damage(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);
        return CurrentHealth <= 0;
    }

    /// <summary>
    /// Heals the entity by a given amount.
    /// </summary>
    public virtual void Heal(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
    }
}
