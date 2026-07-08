using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;
    [SerializeField] private Image attackbarFill;
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject buildingPanelPrefab;
    private Image background;
    private PlayerController playerRef;
    private StatSystem playerStats;

    private TMP_Text alertText;
    private float currentHealth, maxHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerStats = playerRef.GetComponent<StatSystem>();
        currentHealth = playerStats.maxHealth;
        maxHealth = playerStats.maxHealth;
        alertText = GameObject.Find("AlertText").GetComponent<TMP_Text>();
        alertText.text = "";
        background = gameObject.GetComponent<Image>();
        background.enabled = false;
        GetComponent<Canvas>().enabled = false;
    }

    public void StartHud()
    {
        GetComponent<Canvas>().enabled = true;
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

    public IEnumerator AnimateAttackbar(float cooldown)
    {
        float elapsed = 0f;
        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            attackbarFill.fillAmount = elapsed / cooldown;
            yield return null;
        }

        attackbarFill.fillAmount = 1;
    }

    public void Refresh()
    {
        maxHealth = playerStats.maxHealth;
        currentHealth = playerStats.maxHealth;
    }

    public void ToggleBuildMenu(bool state)
    {
        ToggleBackground();
        buildMenu.SetActive(state);
        SpawnPanels(playerRef.GetAvailableBuildings());
    }

    public void StartBuilding(BuildingData building)
    {
        playerRef.StartBuilding(building);
    }

    public void SpawnPanels(BuildingData[] buildingDataArray)
    {
        foreach (Transform child in buildMenu.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var buildingData in buildingDataArray)
        {
            GameObject panel = Instantiate(buildingPanelPrefab, buildMenu.transform);
            BuildingPanelScript panelScript = panel.GetComponent<BuildingPanelScript>();
            panelScript.data = buildingData;
        }
    }

    public void DisplayMessage(string message)
    {
        alertText.text = $"[Alert] {message}";
    }

    private void ToggleBackground()
    {
        background.enabled = !background.enabled;
        var gameSpeed = background.enabled ? 0f : 1f;
        StartCoroutine(UpdateGametime(gameSpeed));
    }

    private IEnumerator UpdateGametime(float speed)
    {
        float targetTimeScale = speed;
        float initialTimeScale = Time.timeScale;
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, elapsed / duration);
            yield return null;
        }
        Time.timeScale = targetTimeScale;
    }
}
