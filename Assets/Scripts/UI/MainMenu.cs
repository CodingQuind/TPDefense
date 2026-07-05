using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject gamemodeObject;
    public GameObject storePage;
    public GameObject mainPage;

    private Gamemode gmControls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gmControls = gamemodeObject.GetComponent<Gamemode>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    // ---Store Interactions---
    public void OpenStore()
    {
        storePage.SetActive(true);
        gmControls.DisablePlayer();
    }

    public void CloseStore()
    {
        storePage.SetActive(false);
        gmControls.EnablePlayer();
    }
}
