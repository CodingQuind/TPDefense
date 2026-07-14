using UnityEngine;
using System.Collections;

public class PlayerLifecycleController : MonoBehaviour
{
    private PlayerController controller;
    private HUDScript hud;
    private Transform respawnPoint;
    private GameObject targetTag;

    public void Initialize()
    {
        controller = GetComponent<PlayerController>();
        hud = GetComponentInChildren<HUDScript>();
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        targetTag = transform.Find("EnemyTargetTag").gameObject;
    }

    public void Tick() { }

    public void Die()
    {
        targetTag.SetActive(false);
        hud.ShowDeathPanel(4f);
        controller.InputHandler.enabled = false;
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
        var controller = GetComponent<PlayerController>();
        controller.Movement.enabled = false;
        controller.Look.LookEnabled = false;
        controller.Combat.enabled = false;
        controller.Building.enabled = false;

        // Unlock cursor for menus
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumePlayer()
    {

        // Re-enable subsystems
        var controller = GetComponent<PlayerController>();
        controller.Movement.enabled = true;
        controller.Look.LookEnabled = true;
        controller.Combat.enabled = true;
        controller.Building.enabled = true;

        // Lock cursor back to gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Respawn()
    {
        controller.InputHandler.enabled = true;
        targetTag.SetActive(true);
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        controller.Combat.Stats.Respawn();
    }
}
