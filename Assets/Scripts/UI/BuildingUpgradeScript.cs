using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UpgradesScript;

public class BuildingUpgradeScript : MonoBehaviour
{
    public UpgradesTable buildingUTable;
    public BuildingUPanel upgradePanelPrefab;
    private List<BuildingUPanel> activePanels = new();

    private GameObject playerRef;
    private PlayerController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        controller = playerRef.GetComponent<PlayerController>();
        RefreshPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshPanel()
    {
        if (activePanels.Count > 0)
        {
            foreach (BuildingUPanel p in activePanels)
            {
                Destroy(p.gameObject);
            }
        }
        activePanels.Clear();
        int count = buildingUTable.upgrades.Length;
        float spacing = 400f;
        float startX = -((count - 1) * spacing) / 2f;
        int i = 0;


        foreach (UpgradeObject bData in buildingUTable.upgrades)
        {
            BuildingUPanel panel = Instantiate(upgradePanelPrefab, transform);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(startX + i * spacing, 0);
            panel.data = bData;
            panel.SetUpgrade(bData, this);
            activePanels.Add(panel);
            i++;
        }
    }

    public UpgradeObject CommitUpgrade(UpgradeObject upgrade, BuildingUPanel panel)
    {
        int availFunds = controller.GetMoney();
        if (availFunds >= upgrade.cost)
        {
            controller.SpendMoney(upgrade.cost);
            controller.AddBuildingUpgrade(upgrade);
            panel.upgradeIcon.sprite = panel.upgradeCompleteIcon;
            panel.purchaseButton.interactable = false;
            panel.purchaseButton.GetComponentInChildren<TMP_Text>().text = "";
            panel.upgradeCost.text = "Purchased";
        }
        return upgrade;
    }
}
