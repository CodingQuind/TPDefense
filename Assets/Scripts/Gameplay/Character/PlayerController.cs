using NUnit.Framework;
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

    [Header("Mouse Look")]
    public float mouseSensitivity = 5f;
    public Camera playerCamera;

    [Header("Combat")]
    [SerializeField] private EClasses playerClass = EClasses.Warrior;
    [SerializeField] private CombatSystem combat;

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
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        ConfigureSettings();

    }

    void Update()
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
            combat.PlayAttackAnimation();
            Ray ray = new(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, combat.GetAttackRange()))
            {
                GameObject objectHit = hit.transform.gameObject;
                if (objectHit.TryGetComponent<IDamageableInterface>(out var damageable))
                {
                    damageable.TakeDamage(this.gameObject, combat.GetPhysicalDamage(), EDamageType.physical);
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
        //Apply damage to stat system
    }
    public void DamageTarget(GameObject target, float damage, EDamageType damageType)
    {
        if (target.TryGetComponent<IDamageableInterface>(out var component)) {
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
        foreach(InputAction action in actions ) 
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

}
