using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUPanel : MonoBehaviour
{
    public UpgradeObject data;

    public Image upgradeIcon;
    public TMP_Text upgradeName;
    public TMP_Text upgradeCost;
    public Button purchaseButton;
    public Sprite upgradeCompleteIcon;

    private PlayerController playerRef;
    void Start()
    {
        

    }

    void Update()
    {
        
    }

    public void SetupPanel()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        upgradeIcon.sprite = data.icon;
        upgradeName.text = data.upgradeName;
        upgradeCost.text = data.cost.ToString() + "g";

    }

    public void SetUpgrade(UpgradeObject upgrade, BuildingUpgradeScript scriptRef)
    {
        this.data = upgrade;
        upgradeIcon.sprite = upgrade.icon;
        upgradeName.text = upgrade.upgradeName;
        upgradeCost.text = upgrade.cost.ToString() + "g";
        purchaseButton.onClick.AddListener(() => scriptRef.CommitUpgrade(upgrade, this));
    }

    
}
