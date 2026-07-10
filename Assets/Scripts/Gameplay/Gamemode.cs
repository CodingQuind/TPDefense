using UnityEngine;

public class Gamemode : MonoBehaviour
{
    public GameObject playerObject;
    private float resourceRegenTimer, regenInterval = 1f;
    private PlayerController controller;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private GameObject enemyPrefab;
    private float enemyTimer = 0f, waveTimer = 10f;
    private SpawnerBehavior spawnControl;
    private bool started;

    private Camera playerCamera, menuCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void Awake()
    {
        controller = playerObject.GetComponent<PlayerController>();
        resourceRegenTimer = 0f;
        started = false;
        playerCamera = controller.gameObject.GetComponentInChildren<Camera>();
        menuCamera = GameObject.FindGameObjectWithTag("Menu Camera").GetComponent<Camera>();
        playerCamera.enabled = false;
        menuCamera.enabled = true;
        DisablePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (started) 
        {    
            resourceRegenTimer += Time.deltaTime;
            enemyTimer += Time.deltaTime;
            if (resourceRegenTimer >= regenInterval)
            {
                resourceRegenTimer = 0;
                controller.Resource.AddMoney(GameSettings.moneyRegenAmt);
            }
            if (enemyTimer >= waveTimer)
            {
                SpawnEnemies();
                enemyTimer = 0f;
            }
        }
    }

    public void DisablePlayer()
    {
        controller.Lifecycle.SuspendPlayer();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void EnablePlayer()
    {
        controller.Lifecycle.ResumePlayer();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartGame()
    {
        started = true;
        controller.Lifecycle.ResumePlayer();
        controller.HUD.StartHud();
        SwapCamera();
    }

    private void SpawnEnemies()
    {
        foreach( GameObject spawner in spawners)
        {
            spawnControl = spawner.GetComponent<SpawnerBehavior>();
            spawnControl.SpawnEnemies(enemyPrefab, 3);
        }
    }

    public void GameOver()
    {
        GameObject.FindGameObjectWithTag("Menu").GetComponent<MainMenu>().GameOver();
        SwapCamera();
    }

    private void SwapCamera()
    {
        playerCamera.enabled = !playerCamera.enabled;
        menuCamera.enabled = !menuCamera.enabled;
    }
}
