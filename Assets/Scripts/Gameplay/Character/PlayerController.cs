using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageableInterface
{
    // --- Public Variables ---
    [Header("Movement")]
    public float baseMoveSpeed = CharacterSettings.characterSpeed;
    public float baseSprintSpeed = CharacterSettings.characterSprintSpeed;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;
    private bool playerEnabled = false;

    [Header("Mouse Look")]
    public float mouseSensitivity = 5f;
    public Camera playerCamera;

    [Header("Combat/Class")]
    [SerializeField] private EClasses playerClass = EClasses.Warrior;
    private CombatSystem combat;
    private StatSystem stats;
    private ResourceSystem resources;
    private GameObject targetTag;

    // --- Private Variables ---
    // Movement and Look
    private CharacterController controller;
    private float verticalVelocity, xRotation;
    private bool isGroundedBuffered;
    private float groundedTimer, groundedBufferTime = 0.15f;
    private float moveAcceleration = 5f, moveSpeed;
    private Vector3 smoothedMove;

    // Input Actions
    private List<InputAction> actions = new();
    private InputAction sprintAction, moveAction, jumpAction;
    private InputAction lookAction;
    private InputAction interactAction, attackAction;

    private Transform respawnPoint;

    public List<UpgradeObject> buildingUpgrades { get; private set; } = new();

    private void ConfigureSettings()
    {

        // Input Action References
        sprintAction = InputSystem.actions.FindAction("sprint");
        moveAction = InputSystem.actions.FindAction("move");
        jumpAction = InputSystem.actions.FindAction("jump");
        interactAction = InputSystem.actions.FindAction("interact");
        lookAction = InputSystem.actions.FindAction("look");
        attackAction = InputSystem.actions.FindAction("Attack");
        actions.Add(sprintAction);
        actions.Add(moveAction);
        actions.Add(jumpAction);
        actions.Add(interactAction);
        actions.Add(lookAction);
        actions.Add(attackAction);
        // Component References
        combat = GetComponent<CombatSystem>();
        stats = GetComponent<StatSystem>();
        resources = GetComponent<ResourceSystem>();
        targetTag = GameObject.Find("enemyTargetTagPlayer");
        stats.InitializeStats();
        playerEnabled = true;
        
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;

    }


    public void StartPlayer()
    {
        ConfigureSettings();
    }

    void Update()
    {
        if (playerEnabled)
        {
            HandleLook();
            HandleMovement();

            if (interactAction.WasPressedThisFrame())
            {
                Ray ray = new(playerCamera.transform.position, playerCamera.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, CharacterSettings.interactRange))
                {
                    GameObject objectHit = hit.transform.gameObject;
                    if (objectHit.TryGetComponent<IInteractInterface>(out var interactable))
                    {
                        interactable.Interact(this.gameObject);
                    }
                }

            }
            if (attackAction.WasPressedThisFrame())
            {
                if (combat.CanAttack())
                {
                    float damage = combat.Attack(EDamageType.physical);
                    Ray ray = new(playerCamera.transform.position, playerCamera.transform.forward);
                    if (Physics.Raycast(ray, out RaycastHit hit, combat.GetAttackRange()))
                    {
                        GameObject objectHit = hit.transform.gameObject;
                        if (objectHit.TryGetComponent<IDamageableInterface>(out var damageable))
                        {
                            Debug.Log("Hitting " + objectHit.name + " for " + damage + " damage.");
                            damageable.TakeDamage(this.gameObject, damage, EDamageType.physical);
                        }
                    }
                }
            }

            
        }
    }

    void HandleLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        // Scale input
        float mouseX = lookInput.x * mouseSensitivity * .01f;
        float mouseY = lookInput.y * mouseSensitivity * .01f;

        // Vertical rotation (camera pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 85f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (player yaw)
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        // --- Ground buffer logic (fixes jump not working sometimes) ---
        if (controller.isGrounded)
        {
            groundedTimer = groundedBufferTime;
        }
        else
        {
            groundedTimer -= Time.deltaTime;
        }
        isGroundedBuffered = groundedTimer > 0f;

        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        smoothedMove = Vector3.Lerp(smoothedMove, move, Time.deltaTime * moveAcceleration);

        // --- Jump ---
        if (isGroundedBuffered)
        {
            if (verticalVelocity < 0) verticalVelocity = -2f;

            if (jumpAction.WasPressedThisFrame())
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- Sprint ---
        float targetSpeed = sprintAction.IsPressed()
                            ? baseSprintSpeed
                            : baseMoveSpeed;

        moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, Time.deltaTime * moveAcceleration);

        // --- Gravity ---
        verticalVelocity += gravity * Time.deltaTime;

        // --- Apply movement once ---
        Vector3 velocity = smoothedMove * moveSpeed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Interact(GameObject interactObject)
    {
        Debug.Log("Hit " + interactObject.name);
    }

    public void TakeDamage(GameObject attacker, float damage, EDamageType damageType)
    {
        if (stats.Damage(damage))
        {
            KillSelf();
        }
    }
    public void DamageTarget(GameObject target, float damage, EDamageType damageType)
    {
        if (target.TryGetComponent<IDamageableInterface>(out var component))
        {
            component.TakeDamage(this.gameObject, damage, damageType);
        }
    }

    public void EnablePlayer()
    {
        foreach (InputAction action in actions)
        {
            action.Enable();
        }
    }

    public void DisablePlayer()
    {
        foreach (InputAction action in actions)
        {
            action.Disable();
        }
    }

    public void SelectClass(EClasses newClass)
    {
        playerClass = newClass;
    }

    public EClasses GetClass()
    {
        return playerClass;
    }

    public int GetMoney()
    {
        return resources.Money();
    }

    public void SpendMoney(int amt)
    {
        resources.RemoveMoney(amt);
    }

    public void ApplyStatUpgrade(UpgradeObject upgrade)
    {
        stats.AddUpgrade(upgrade);
    }

    public void ApplyBuildingUpgrade(UpgradeObject upgrade)
    {
        buildingUpgrades.Add(upgrade);
        foreach (BuildingBehavior building in FindObjectsByType<BuildingBehavior>())
        {
            building.ApplyUpgrade(upgrade);
        }
    }

    private void KillSelf()
    {
        Debug.Log("Player has died.");
        foreach (InputAction action in actions) 
        {
            action.Disable();
        }
        StartCoroutine(RespawnTimer(4f));

    }

    private float respawnGracePeriod = 3f;
    private IEnumerator RespawnTimer(float respawnTime)
    {
        Debug.Log("Player respawning in " + respawnTime + " seconds.");
        StartCoroutine(GracePeriod(respawnTime + respawnGracePeriod));
        yield return new WaitForSeconds(respawnTime);
        Respawn();
    }

    public void Respawn()
    {
        Debug.Log("Player has respawned.");
        gameObject.transform.position = respawnPoint.position;
        gameObject.transform.rotation = respawnPoint.rotation;
        foreach (InputAction action in actions)
        {
            action.Enable();
        }
        
    }

    private IEnumerator GracePeriod(float graceTime)
    {
        Debug.Log("Grace period started for " + graceTime + " seconds.");
        targetTag.SetActive(false);
        yield return new WaitForSeconds(graceTime);
        targetTag.SetActive(true);
        Debug.Log("Grace period ended.");
    }
}
