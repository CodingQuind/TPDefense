using UnityEngine;
using UnityEngine.UI;

public class BuildingPanelScript : MonoBehaviour
{
    public BuildingData data;
    public Image buildingIcon;
    public TMPro.TMP_Text buildingName;
    public TMPro.TMP_Text buildingCost;
    public Button buildButton;
    public Sprite upgradeCompleteIcon;

    private PlayerController controller;
    private HUDScript hud;

    public void TryBuild()
    {
        var money = controller.Resource.Money;
        
        if (money >= data.buildingData.buildingCost)
        {
            controller.Resource.SpendMoney(data.buildingData.buildingCost);
            controller.Building.StartBuilding(data);
        }
        else
        {
            hud.DisplayMessage("Not enough money to build " + data.buildingData.buildingName + "!");
            
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        hud = controller.HUD;

        buildingIcon.sprite = data.buildingData.buildingIcon;
        buildingName.text = data.buildingData.buildingName;
        buildingCost.text = data.buildingData.buildingCost.ToString() + "g";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
