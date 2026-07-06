using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [Header("Stat System")]
    [SerializeField] private StatSystem stats;

    [Header("Animation")]
    public GameObject weaponRoot;

    private int physicalDmg, magicDmg;
    private float attackRange = 4f, castTimer = 0f, cooldown = 1f;
    public bool attacking { get; private set; } = false;

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
        if (castTimer < cooldown)
        {
            castTimer += Time.deltaTime;
        }
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

    public bool CanAttack()
    {
        return castTimer >= cooldown && !attacking;
    }

    public float Attack(EDamageType type)
    {
        if (castTimer >= cooldown & !attacking)
        {
            attacking = true;
            PlayAttackAnimation();
            castTimer = 0f;
            return type == EDamageType.physical ? GetPhysicalDamage() : GetMagicDamage();
        }
        return 0f;
    }
    private void PlayAttackAnimation()
    {
        if (weaponRoot != null)
        {
            StartCoroutine("AnimateAttack");
        }
    }


    private System.Collections.IEnumerator AnimateAttack()
    {
        // Simple attack animation: rotate the weapon root back and forth
        float animationDuration = .3f; // Duration of the attack animation
        float elapsedTime = 0f;
        Quaternion initialRotation = weaponRoot.transform.localRotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(45f, 0f, 0f);
        while (elapsedTime < animationDuration)
        {
            weaponRoot.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset rotation after attack
        weaponRoot.transform.localRotation = initialRotation;
        attacking = false;
    }
}
