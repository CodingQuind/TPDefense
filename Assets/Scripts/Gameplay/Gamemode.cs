using UnityEngine;

public class Gamemode : MonoBehaviour
{
    public ResourceSystem resourceSystem;
    public GameObject playerObject;
    private float resourceRegenTimer, regenInterval = 1f;
    private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resourceRegenTimer = 0f;
        resourceSystem.Start();
        playerController = playerObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        resourceRegenTimer += Time.deltaTime;
        if (resourceRegenTimer >= regenInterval)
        {
            resourceRegenTimer = 0;
            resourceSystem.AddMoney(GameSettings.moneyRegenAmt);
            Debug.Log(resourceSystem.Money());
        }
    }

    public void DisablePlayer()
    {
        playerController.DisablePlayer();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void EnablePlayer()
    {
        playerController.EnablePlayer();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
