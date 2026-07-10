using UnityEngine;
using System.Collections;

public class PlayerLifecycleController : MonoBehaviour
{
    private PlayerStatSystem stats;
    private HUDScript hud;
    private Transform respawnPoint;

    public void Initialize()
    {
        stats = GetComponent<PlayerStatSystem>();
        hud = GetComponentInChildren<HUDScript>();
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
    }

    public void Tick() { }

    public void KillPlayer()
    {
        hud.ShowDeathPanel(4f);
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(4f);
        Respawn();
    }

    public void SuspendPlayer()
    {

        // Disable movement/look/combat/building
        GetComponent<PlayerMovementController>().enabled = false;
        GetComponent<PlayerLookController>().LookEnabled = false;
        GetComponent<PlayerCombatController>().enabled = false;
        GetComponent<PlayerBuildingController>().enabled = false;

        // Unlock cursor for menus
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumePlayer()
    {

        // Re-enable subsystems
        GetComponent<PlayerMovementController>().enabled = true;
        GetComponent<PlayerLookController>().LookEnabled = true;
        GetComponent<PlayerCombatController>().enabled = true;
        GetComponent<PlayerBuildingController>().enabled = true;

        // Lock cursor back to gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Respawn()
    {
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        stats.Respawn();
        hud.Refresh();
    }
}
