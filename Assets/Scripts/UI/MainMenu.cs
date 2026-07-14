using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject gamemodeObject;
    public GameObject storePage;
    public GameObject mainPage;
    public GameObject debugPanel;
    public GameObject helpPage;
    public GameObject gameOverPage;
    public GameObject pauseMenu;

    private Gamemode gmControls;
    private InputAction debugKey, backKey;
    private PlayerController playerRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gmControls = gamemodeObject.GetComponent<Gamemode>();
        debugKey = InputSystem.actions.FindAction("Open Debug");
        backKey = InputSystem.actions.FindAction("Back");
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (debugKey.triggered)
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }

        if (backKey.triggered)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseMenu.activeSelf)
            {
                gmControls.DisablePlayer();
                playerRef.HUD.HideOverlay();
            }
            else
            {
                gmControls.EnablePlayer();
                playerRef.HUD.ShowOverlay();
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainPage.SetActive(false);
        helpPage.SetActive(false);
        gmControls.StartGame();
        
    }

    // ---Store Interactions---
    public void OpenStore()
    {
        playerRef.HUD.HideOverlay();
        storePage.SetActive(true);
        gmControls.DisablePlayer();
        gmControls.camManager.ShowMainMenu();
        Time.timeScale = 0f;
    }

    public void CloseStore()
    {
        Time.timeScale = 1f;
        playerRef.HUD.ShowOverlay();
        storePage.SetActive(false);
        gmControls.camManager.ShowGameplay();
        gmControls.EnablePlayer();
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOverPage.SetActive(true);
        gmControls.DisablePlayer();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        gmControls.EnablePlayer();
        playerRef.HUD.ShowOverlay();
    }
}
