using UnityEngine;

public class Gamemode : MonoBehaviour
{
    private float resourceRegenTimer, regenInterval = 1f;
    private PlayerController controller;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private GameObject enemyPrefab;
    private float enemyTimer = 0f, waveTimer = 10f;
    private SpawnerBehavior spawnControl;
    private bool started;

    public CameraManager camManager { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camManager.ShowMainMenu();
        DisablePlayer();
    }

    void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        resourceRegenTimer = 0f;
        started = false;
        camManager = GetComponent<CameraManager>();

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
        camManager.ShowGameplay();
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
    }

}
