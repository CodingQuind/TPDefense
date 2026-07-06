using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradesScript : MonoBehaviour
{
    public List<UpgradeEntry> entries;
    public Dictionary<EClasses, UpgradesTable> upgradeTables = new();
    public UpgradePanel upgradePanelPrefab;
    private List<UpgradePanel> activePanels = new();
    private EClasses curClass = EClasses.Warrior;
    private UpgradesTable upgradeTable;

    private GameObject playerRef;
    private PlayerController controller;
    void Start()
    {

        foreach (var entry in entries)
            upgradeTables[entry.key] = entry.value;

        playerRef = GameObject.FindGameObjectWithTag("Player");
        controller = playerRef.GetComponent<PlayerController>();
        curClass = controller.GetClass();
        upgradeTable = upgradeTables[curClass];
        RefreshPanel();
    }

    void Update()
    {
        
    }

    public void RefreshClass() 
    { 
        curClass = controller.GetClass();
        upgradeTable = upgradeTables[curClass];
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
        RefreshClass();
        int count = upgradeTable.upgrades.Length;
        float spacing = 400f;
        float startX = -((count - 1) * spacing) / 2f;
        int i = 0;


        foreach (UpgradeObject upgrade in upgradeTable.upgrades)
        {
            UpgradePanel panel = Instantiate(upgradePanelPrefab, transform);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(startX + i * spacing, 0);
            panel.SetUpgrade(upgrade, this);
            activePanels.Add(panel);
            i++;
        }
    }
    [System.Serializable]
    public struct UpgradeEntry
    {
        public EClasses key;
        public UpgradesTable value;
    }

    public UpgradeObject CommitUpgrade(UpgradeObject upgrade, UpgradePanel panel)
    {
        int availFunds = controller.GetMoney();
        if (availFunds >= upgrade.cost)
        {
            controller.SpendMoney(upgrade.cost);
            controller.ApplyStatUpgrade(upgrade);
            panel.upgradeIcon.sprite = panel.upgradeCompleteIcon;
            panel.upgradeButton.interactable = false;
            panel.upgradeButton.GetComponentInChildren<TMP_Text>().text = "";
            panel.upgradeCostText.text = "Purchased";
        }
        return upgrade;
    }
}


