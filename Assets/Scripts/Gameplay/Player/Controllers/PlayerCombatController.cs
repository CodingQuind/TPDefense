using UnityEngine;

public class PlayerCombatController : MonoBehaviour, IDamageableInterface
{
    private PlayerInputHandler input;
    private PlayerStatSystem stats;
    private PlayerAnimationController animator;
    private HUDScript hud;
    private Camera cam;

    private float attackTimer;
    private float regenTimer;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        stats = GetComponent<PlayerStatSystem>();
        animator = GetComponent<PlayerAnimationController>();
        hud = GetComponentInChildren<HUDScript>();
        cam = GetComponentInChildren<Camera>();
    }

    public void Tick()
    {
        HandleRegen();
        HandleAttack();
    }

    private void HandleRegen()
    {
        if (regenTimer >= StatSystemSettings.regenerationDelay)
            stats.Heal(StatSystemSettings.regenerationRate * Time.deltaTime);
        else
            regenTimer += Time.deltaTime;
    }

    private void HandleAttack()
    {
        attackTimer += Time.deltaTime;

        if (!input.AttackPressed || attackTimer < CharacterSettings.attackSpeed)
            return;

        attackTimer = 0f;
        StartCoroutine(hud.AnimateAttackbar(CharacterSettings.attackSpeed));
        animator.PlayAttackAnimation("axe"); // <--- will need to be replaced with active weapon

        Ray ray = new(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, CharacterSettings.attackRange))
        {
            if (hit.transform.TryGetComponent<IDamageableInterface>(out var dmg))
                dmg.TakeDamage(gameObject, stats.GetPhysicalDamage(), EDamageType.physical);
        }
    }

    public float GetDamage()
    {
        return stats.GetPhysicalDamage();
    }

    public void TakeDamage(GameObject source, float amount, EDamageType type)
    {
        stats.Damage(amount);
        regenTimer = 0f;
    }

    public void Heal(float amount)
    {
        stats.Heal(amount);
    }

    public void DamageTarget(GameObject target, float damage, EDamageType damageType) { }

    public EClasses GetClass()
    {
        return stats.Class;
    }

    public void AddUpgrade(UpgradeObject upgrade)
    {
        stats.AddUpgrade(upgrade);
    }

    public float GetHealth() { return stats.CurrentHealth; }
    public float GetMaxHealth() { return stats.MaxHealth; }
}
