using UnityEngine;

public class Gamemode : MonoBehaviour
{
    public GameObject playerObject;
    private float resourceRegenTimer, regenInterval = 1f;
    private PlayerController playerController;
    private ResourceSystem resourceSystem;
    private bool started;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = playerObject.GetComponent<PlayerController>();
        resourceSystem = playerController.gameObject.GetComponent<ResourceSystem>();
        resourceRegenTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (started) 
        {    
            resourceRegenTimer += Time.deltaTime;
            if (resourceRegenTimer >= regenInterval)
            {
                resourceRegenTimer = 0;
                resourceSystem.AddMoney(GameSettings.moneyRegenAmt);
            }
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

    public void StartGame()
    {
        resourceSystem.Start();
        started = true;
    }
}
