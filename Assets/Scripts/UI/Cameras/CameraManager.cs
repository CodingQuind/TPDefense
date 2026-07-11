using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera References")]
    public Camera gameplayCamera;
    public Camera mainMenuCamera;

    [Header("Settings")]
    public bool syncPositionOnMenuOpen = true;
    public bool overlayWorldBehindMenu = false;

    private void Awake()
    {
        if (gameplayCamera == null || mainMenuCamera == null)
        {
            Debug.LogWarning("CameraManager: Missing camera references.");
        }
    }

    /// <summary>
    /// Activates the main menu camera and disables gameplay camera.
    /// Optionally syncs position and rotation to gameplay camera.
    /// </summary>
    public void ShowMainMenu()
    {
        if (syncPositionOnMenuOpen && gameplayCamera && mainMenuCamera)
        {
            mainMenuCamera.transform.position = gameplayCamera.transform.position;
            mainMenuCamera.transform.rotation = gameplayCamera.transform.rotation;
        }

        if (overlayWorldBehindMenu)
        {
            gameplayCamera.enabled = true;
            gameplayCamera.depth = 0;
            mainMenuCamera.enabled = true;
            mainMenuCamera.depth = 1;
        }
        else
        {
            gameplayCamera.enabled = false;
            mainMenuCamera.enabled = true;
        }
    }

    /// <summary>
    /// Returns to gameplay camera view.
    /// </summary>
    public void ShowGameplay()
    {
        mainMenuCamera.enabled = false;
        gameplayCamera.enabled = true;
    }

    /// <summary>
    /// Toggles between menu and gameplay cameras.
    /// </summary>
    public void ToggleMenu()
    {
        if (mainMenuCamera.enabled)
            ShowGameplay();
        else
            ShowMainMenu();
    }
}
