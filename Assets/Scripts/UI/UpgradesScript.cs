using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesScript : MonoBehaviour
{
    public UpgradesTable[] upgradeTables;
    public UpgradePanel upgradePanelPrefab;
    private List<UpgradePanel> activePanels = new();
    private EClasses curClass = EClasses.Warrior;
    private UpgradesTable upgradeTable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgradeTable = upgradeTables[0];
        foreach (UpgradesTable uT in upgradeTables)
        {
            upgradeTable = uT.GetName() == curClass.ToString() ? uT : upgradeTable;
        }

        RefreshPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RefreshClass(EClasses eClass) { curClass = eClass; }

    private void RefreshPanel()
    {
        foreach (UpgradePanel p in activePanels)
        {
            Destroy(p.gameObject);
        }

        int count = upgradeTable.upgrades.Length;
        float spacing = 200f;
        float startX = -((count - 1) * spacing) / 2f;
        int i = 0;


        foreach (Upgrade upgrade in upgradeTable.upgrades)
        {
            UpgradePanel panel = Instantiate(upgradePanelPrefab, transform);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(startX + i * spacing, 0);
            panel.SetUpgrade(upgrade);
            activePanels.Add(panel);
            i++;
        }
    }
}
