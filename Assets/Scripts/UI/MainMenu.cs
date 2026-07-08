using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject gamemodeObject;
    public GameObject storePage;
    public GameObject mainPage;
    public GameObject debugPanel;

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
        gmControls.StartGame();
    }

    // ---Store Interactions---
    public void OpenStore()
    {
        playerRef.HideHud();
        storePage.SetActive(true);
        gmControls.DisablePlayer();
    }

    public void CloseStore()
    {
        playerRef.ShowHud();
        storePage.SetActive(false);
        gmControls.EnablePlayer();
    }
}
