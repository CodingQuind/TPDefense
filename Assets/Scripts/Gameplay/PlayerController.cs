using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 300f;
    public Transform cameraTransform;

    private CharacterController controller;
    private float verticalVelocity;
    private float xRotation;

    private bool isGroundedBuffered;
    private float groundedTimer;
    private float groundedBufferTime = 0.15f;
    private float moveAcceleration = 5f;
    private Vector3 smoothedMove;

    private InputAction sprintAction;
    private InputAction moveAction;
    private InputAction jumpAction;

    private void ConfigureSettings()
    {
        moveSpeed = CharacterSettings.characterSpeed;

        // Input Action References
        sprintAction = InputSystem.actions.FindAction("sprint");
        moveAction = InputSystem.actions.FindAction("move");
        jumpAction = InputSystem.actions.FindAction("jump");
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        ConfigureSettings();
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 85f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
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
                            ? CharacterSettings.characterSprintSpeed 
                            : CharacterSettings.characterSpeed;

        moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, Time.deltaTime * moveAcceleration);

        // --- Gravity ---
        verticalVelocity += gravity * Time.deltaTime;

        // --- Apply movement once ---
        Vector3 velocity = smoothedMove * moveSpeed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
}
