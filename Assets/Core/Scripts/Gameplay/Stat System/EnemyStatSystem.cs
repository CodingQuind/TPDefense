using UnityEngine;

/// <summary>
/// Stat system for enemies. Handles enemy health, damage, and scaling.
/// </summary>
public class EnemyStatSystem : BaseStatSystem
{
    [Header("Enemy Stats")]
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float baseSpeed = 3.5f;

    public override void InitializeStats()
    {
        MaxHealth = GameSettings.Instance.StatSystemSettings.defaultEnemyHealth;
        MaxEnergy = 0; // enemies don't use energy

        base.InitializeStats();
    }

    public override float GetPhysicalDamage()
    {
        return baseDamage;
    }

    public float GetSpeed()
    {
        return baseSpeed;
    }
}
