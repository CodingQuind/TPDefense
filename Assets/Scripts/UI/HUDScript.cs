using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;
    private StatSystem playerStats;
    private float currentHealth, maxHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<StatSystem>();
        currentHealth = playerStats.maxHealth;
        maxHealth = playerStats.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float newHealth = playerStats.currentHealth;
        if (currentHealth - newHealth != 0)
        {
            StartCoroutine(AnimateHealth(newHealth));
        }
    }

    private IEnumerator AnimateHealth(float newHealth)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        float startHealth = currentHealth;
        float targetHealth = Mathf.Clamp(newHealth, 0, maxHealth);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentHealth = Mathf.Lerp(startHealth, targetHealth, elapsed / duration);
            healthFillImage.fillAmount = currentHealth / maxHealth;
            yield return null;
        }

        currentHealth = targetHealth;
        healthFillImage.fillAmount = currentHealth / maxHealth;
    }

    public void Refresh()
    {
        maxHealth = playerStats.maxHealth;
        currentHealth = playerStats.maxHealth;
    }
}
