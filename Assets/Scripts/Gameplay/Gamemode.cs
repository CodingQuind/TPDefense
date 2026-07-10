using UnityEngine;

public class Gamemode : MonoBehaviour
{
    public GameObject playerObject;
    private float resourceRegenTimer, regenInterval = 1f;
    private PlayerController playerController;
    private PlayerLifecycleController playerLCController;
    private ResourceSystem resourceSystem;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private GameObject enemyPrefab;
    private float enemyTimer = 0f, waveTimer = 10f;
    private SpawnerBehavior spawnControl;
    private bool started;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = playerObject.GetComponent<PlayerController>();
        resourceSystem = playerController.gameObject.GetComponent<ResourceSystem>();
        playerLCController = playerController.gameObject.GetComponent<PlayerLifecycleController>();
        resourceRegenTimer = 0f;
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
                resourceSystem.AddMoney(GameSettings.moneyRegenAmt);
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
        playerLCController.SuspendPlayer();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void EnablePlayer()
    {
        playerLCController.ResumePlayer();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartGame()
    {
        resourceSystem.Start();
        started = true;
        playerLCController.ResumePlayer();
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
