using System;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    private Upgrade upgrade;
    public Image upgradeIcon;
    public TMPro.TMP_Text upgradeNameText;
    public TMPro.TMP_Text upgradeCostText;

    public void SetUpgrade(Upgrade upgrade)
    {
        this.upgrade = upgrade;
        upgradeIcon.sprite = upgrade.icon;
        upgradeNameText.text = upgrade.upgradeName;
        upgradeCostText.text = upgrade.cost.ToString() + "g";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
