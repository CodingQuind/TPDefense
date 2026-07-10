using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDHealthController : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private Image attackFill;

    private PlayerStatSystem stats;
    private float currentHealth;

    void Awake()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatSystem>();
        currentHealth = stats.CurrentHealth;
    }

    public void Refresh()
    {
        currentHealth = stats.CurrentHealth;
        healthFill.fillAmount = currentHealth / stats.MaxHealth;
    }

    public void AnimateHealth(float newHealth)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateHealthRoutine(newHealth));
    }

    private IEnumerator AnimateHealthRoutine(float newHealth)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        float start = currentHealth;
        float target = Mathf.Clamp(newHealth, 0, stats.MaxHealth);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentHealth = Mathf.Lerp(start, target, elapsed / duration);
            healthFill.fillAmount = currentHealth / stats.MaxHealth;
            yield return null;
        }

        currentHealth = target;
        healthFill.fillAmount = currentHealth / stats.MaxHealth;
    }

    public IEnumerator AnimateAttackBar(float cooldown)
    {
        float elapsed = 0f;
        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            attackFill.fillAmount = elapsed / cooldown;
            yield return null;
        }
        attackFill.fillAmount = 1;
    }
}
