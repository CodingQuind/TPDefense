using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [Header("Stat System")]
    [SerializeField] private StatSystem stats;

    private int physicalDmg, magicDmg;
    private float attackRange = 2f, castTimer = 0f, cooldown = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (stats == null) { stats = gameObject.AddComponent<StatSystem>(); }

        physicalDmg = stats.GetStrength() * 10 + stats.GetPhysicalDamage();
        magicDmg = stats.GetIntelligence() * 10 + stats.GetIntelligence();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetPhysicalDamage()
    {
        return physicalDmg;
    }

    public float GetMagicDamage()
    {
        return magicDmg;
    }
}
