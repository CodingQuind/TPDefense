using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerMovementController Movement { get; private set; }
    public PlayerLookController Look { get; private set; }
    public PlayerCombatController Combat { get; private set; }
    public PlayerBuildingController Building { get; private set; }
    public PlayerLifecycleController Lifecycle { get; private set; }
    public PlayerResourceController Resource { get; private set; }
    public HUDScript HUD { get; private set; }

    private void Awake()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        Movement = GetComponent<PlayerMovementController>();
        Look = GetComponent<PlayerLookController>();
        Combat = GetComponent<PlayerCombatController>();
        Building = GetComponent<PlayerBuildingController>();
        Lifecycle = GetComponent<PlayerLifecycleController>();
        Resource = GetComponent<PlayerResourceController>();
        HUD = gameObject.transform.GetComponentInChildren<HUDScript>();
    }

    private void Start()
    {
        InputHandler.Initialize();
        Lifecycle.Initialize();
    }

    private void Update()
    {
        Movement.Tick();
        Look.Tick();
        Combat.Tick();
        Building.Tick();
        Lifecycle.Tick();
    }
}
