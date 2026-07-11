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

    private Gamemode gmControls;
    private InputAction debugKey;
    private PlayerController playerRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gmControls = gamemodeObject.GetComponent<Gamemode>();
        debugKey = InputSystem.actions.FindAction("Open Debug");
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (debugKey.triggered)
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
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
}
