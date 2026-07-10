using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;
    [SerializeField] private Image attackbarFill;
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject buildingPanelPrefab;
    [SerializeField] private TMP_Text moneyCount;

    private Image background;
    private PlayerController controller;
    private GameObject overlay;
    private GameObject deathPanel, pausePanel;
    private float currentHealth, maxHealth;
    private TMP_Text alertText;
    private GameObject buildButton;

    
    void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        alertText = GameObject.Find("AlertText").GetComponent<TMP_Text>();
        alertText.text = "";
        background = gameObject.GetComponent<Image>();
        background.enabled = false;
        GetComponent<Canvas>().enabled = false;
        overlay = GameObject.Find("Overlay");
        overlay.SetActive(false);
        buildButton = GameObject.Find("BuildButton");
        deathPanel = GameObject.Find("DeathPanel");
        deathPanel.SetActive(false);
        pausePanel = GameObject.Find("PauseMenu");
        pausePanel.SetActive(false);

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Refresh();
    }

    public void StartHud()
    {
        GetComponent<Canvas>().enabled = true;
        overlay.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        float newHealth = controller.Combat.GetHealth();
        if (currentHealth - newHealth != 0)
        {
            StartCoroutine(AnimateHealth(newHealth));
        }
        if (controller != null) moneyCount.text = $"Money: {controller.Resource.Money}g";

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
        maxHealth = controller.Combat.GetHealth();
        currentHealth = controller.Combat.GetHealth();
    }

    public void ToggleBuildMenu(bool state)
    {
        ToggleBackground();
        buildMenu.SetActive(state);
        SpawnPanels(controller.Building.BuildingList);
        
    }

    public void SpawnPanels(List<BuildingData> buildings)
    {
        foreach (Transform child in buildMenu.transform)
        {
            Destroy(child.gameObject);
        }

        float totalWidth = 600f; // width of menu area
        float spacing = totalWidth / (buildings.Count);
        float startX = -totalWidth / 2f + 110f;

        for (int i = 0; i < buildings.Count; i++)
        {
            GameObject panel = Instantiate(buildingPanelPrefab, buildMenu.transform);
            var rect = panel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(startX + spacing * (i + 1), 0);

            var panelScript = panel.GetComponent<BuildingPanelScript>();
            panelScript.data = buildings[i];
        }
    }

    public void DisplayMessage(string message)
    {
        alertText.text = $"[Alert] {message}";
        var fadeColor = new Color(alertText.color.r, alertText.color.g, alertText.color.b, 0f);
        alertText.CrossFadeColor(fadeColor, 3f, true, true);
        StartCoroutine(ResetAlert(3f));

    }

    private IEnumerator ResetAlert(float waitTime)
    {
        var elapsed = 0f;
        while (elapsed < waitTime)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        alertText.text = "";
        var fullColor = new Color(alertText.color.r, alertText.color.g, alertText.color.b, 1f);
        alertText.CrossFadeColor(fullColor, 0f, true, true);

    }

    private void ToggleBackground()
    {
        background.enabled = !background.enabled;
        var gameSpeed = background.enabled ? 0f : 1f;
        StartCoroutine(UpdateGametime(gameSpeed));
        overlay.SetActive(!overlay.activeSelf);

    }

    public void ShowOverlay()
    {
        overlay.SetActive(true);
        buildButton.SetActive(true);
        
    }

    public void HideOverlay()
    {
        buildButton.SetActive(false);
        overlay.SetActive(false);
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

    public void ShowDeathPanel(float duration)
    {
        deathPanel.SetActive(true);
        overlay.SetActive(false);
        buildButton.SetActive(false);
        StartCoroutine(DeathPanelTimer(duration));
    }

    public TMP_Text respawnText;
    private IEnumerator DeathPanelTimer(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            if (elapsed % 1f < 0.1f)
            {
                respawnText.text = $"Respawning in {Mathf.CeilToInt(duration - elapsed)} seconds...";
            }
            yield return null;
        }
        HideDeathPanel();
    }
    public void HideDeathPanel()
    {
        deathPanel.SetActive(false);
        overlay.SetActive(true);
        buildButton.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        ToggleBackground();
        HideOverlay();
        pausePanel.SetActive(true);
        controller.Lifecycle.SuspendPlayer();
    }

    public void HidePauseMenu()
    {
        ToggleBackground();
        ShowOverlay();
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller.Lifecycle.ResumePlayer();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
