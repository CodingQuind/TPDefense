using UnityEngine;

public interface IDamageableInterface
{
    public void TakeDamage(GameObject attacker, float damage, EDamageType damageType);
    public void DamageTarget(GameObject target, float damage, EDamageType damageType);
}
