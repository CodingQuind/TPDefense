using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UpgradesScript;

public class BuildingUpgradeScript : MonoBehaviour
{
    public List<UpgradeEntry> entries;
    public UpgradesTable buildingUTable;
    public UpgradePanel upgradePanelPrefab;
    private List<UpgradePanel> activePanels = new();

    private GameObject playerRef;
    private PlayerController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("player");
        controller = playerRef.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshPanel()
    {
        if (activePanels.Count > 0)
        {
            foreach (UpgradePanel p in activePanels)
            {
                Destroy(p.gameObject);
            }
        }
        activePanels.Clear();
        int count = buildingUTable.upgrades.Length;
        float spacing = 400f;
        float startX = -((count - 1) * spacing) / 2f;
        int i = 0;


        foreach (UpgradeObject upgrade in buildingUTable.upgrades)
        {
            UpgradePanel panel = Instantiate(upgradePanelPrefab, transform);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(startX + i * spacing, 0);
            panel.SetUpgrade(upgrade, this);
            activePanels.Add(panel);
            i++;
        }
    }

    public UpgradeObject CommitUpgrade(UpgradeObject upgrade, UpgradePanel panel)
    {
        int availFunds = controller.GetMoney();
        if (availFunds >= upgrade.cost)
        {
            controller.SpendMoney(upgrade.cost);
            controller.ApplyBuildingUpgrade(upgrade);
            panel.upgradeIcon.sprite = panel.upgradeCompleteIcon;
            panel.upgradeButton.interactable = false;
            panel.upgradeButton.GetComponentInChildren<TMP_Text>().text = "";
            panel.upgradeCostText.text = "Purchased";
        }
        return upgrade;
    }
}
