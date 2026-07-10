using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    private PlayerInputHandler input;
    private PlayerStatSystem stats;
    private HUDScript hud;
    private Camera cam;

    private float attackTimer;
    private float regenTimer;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        stats = GetComponent<PlayerStatSystem>();
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

    public EClasses GetClass()
    {
        return stats.Class;
    }

    public void AddUpgrade(UpgradeObject upgrade)
    {
        stats.AddUpgrade(upgrade);
    }
}
