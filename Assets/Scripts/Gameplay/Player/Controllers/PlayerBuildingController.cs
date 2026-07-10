using UnityEngine;

public class PlayerBuildingController : MonoBehaviour
{
    private PlayerInputHandler input;
    private HUDScript hud;
    private Camera cam;

    private bool buildMode;
    private GameObject ghost;
    private BuildingData currentData;
    public BuildingData[] BuildingList { get; private set; }

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        hud = GetComponentInChildren<HUDScript>();
        cam = GetComponentInChildren<Camera>();
    }

    public void Tick()
    {
        if (input.BuildPressed)
            ToggleBuildMode();

        if (ghost != null)
            UpdateGhostPosition();
    }

    private void ToggleBuildMode()
    {
        buildMode = !buildMode;
        hud.ToggleBuildMenu(buildMode);
        Cursor.visible = buildMode;
        Cursor.lockState = buildMode ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void UpdateGhostPosition()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit))
            ghost.transform.position = hit.point + Vector3.down * 1.1f;

        if (Input.GetMouseButtonDown(0))
            PlaceBuilding();
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
        currentData = data;
        ghost = Instantiate(data.buildingData.ghostPrefab);
    }
}
