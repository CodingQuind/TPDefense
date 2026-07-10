using UnityEngine;
using System.Collections.Generic;

public class PlayerBuildingController : MonoBehaviour
{
    private PlayerController controller;
    private PlayerInputHandler input;
    private HUDScript hud;
    private Camera cam;

    private bool buildMode = false;
    private GameObject ghost = null;
    private BuildingData currentData;
    private float rotateSnapDegrees = 15f;
    public float BuildRange { get; private set; }
    public BuildingData[] startingBuildings;
    public List<BuildingData> BuildingList { get; private set; } = new List<BuildingData>();
    private UpgradeObject[] activeUpgrades = new UpgradeObject[0];

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        input = GetComponent<PlayerInputHandler>();
        hud = GetComponentInChildren<HUDScript>();
        cam = GetComponentInChildren<Camera>();
        foreach (BuildingData data in startingBuildings) AddBuilding(data);
        BuildRange = BuildSystemSettings.buildRange;
    }

    public void Tick()
    {
        if (input.BuildPressed)
            ToggleBuildMode();

        if (ghost != null)
            UpdateGhostPosition();
            UpdateGhostRotation();
    }

    private void ToggleBuildMode()
    {
        buildMode = !buildMode;
        hud.ToggleBuildMenu(buildMode);
        switch (buildMode) 
        {
            case true:
                controller.Lifecycle.SuspendPlayer();
                break;
            case false:
                controller.Lifecycle.ResumePlayer();
                break; 
        }
    }

    private void UpdateGhostPosition()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, BuildRange, LayerMask.GetMask("Ground")))
            ghost.transform.position = hit.point + Vector3.down * 1.1f;
        if (Input.GetMouseButtonDown(0))
            PlaceBuilding();
    }

    private void UpdateGhostRotation()
    {
        float scroll = input.ScrollInput;

        if (scroll > 0.1f)
            ghost.transform.Rotate(Vector3.up, rotateSnapDegrees);

        if (scroll < -0.1f)
            ghost.transform.Rotate(Vector3.up, -rotateSnapDegrees);
    }

    private void PlaceBuilding()
    {
        Instantiate(currentData.buildingData.buildingPrefab, ghost.transform.position, ghost.transform.rotation);
        Destroy(ghost);
        ghost = null;
        buildMode = false;
    }

    public void StartBuilding(BuildingData data)
    {
        ToggleBuildMode();
        currentData = data;
        ghost = Instantiate(data.buildingData.ghostPrefab);
    }

    public void AddUpgrade(UpgradeObject upgrade)
    {
        var newUpgrades = new UpgradeObject[activeUpgrades.Length + 1];
        for (int i = 0; i < activeUpgrades.Length; i++)
            newUpgrades[i] = activeUpgrades[i];
        newUpgrades[activeUpgrades.Length] = upgrade;
        activeUpgrades = newUpgrades;
    }

    public void AddBuilding(BuildingData data) 
    {
        BuildingList.Add(data);
    }
}
