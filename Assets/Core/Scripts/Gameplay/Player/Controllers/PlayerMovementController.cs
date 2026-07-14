using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerInputHandler input;
    private CharacterController controller;

    private float verticalVelocity;
    private float groundedTimer;
    private bool isGroundedBuffered;

    private float moveSpeed;
    private Vector3 smoothedMove;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        controller = GetComponent<CharacterController>();
    }

    public void Tick()
    {
        HandleGroundBuffer();
        HandleMovement();
        HandleJump();
        ApplyGravity();
        ApplyMovement();
    }

    private void HandleGroundBuffer()
    {
        if (controller.isGrounded)
            groundedTimer = 0.15f;
        else
            groundedTimer -= Time.deltaTime;

        isGroundedBuffered = groundedTimer > 0f;
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * input.MoveInput.x + transform.forward * input.MoveInput.y;
        smoothedMove = Vector3.Lerp(smoothedMove, move, Time.deltaTime * 5f);

        float targetSpeed = input.SprintHeld ? GameSettings.Instance.CharacterSettings.characterSprintSpeed : GameSettings.Instance.CharacterSettings.characterSpeed;
        moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, Time.deltaTime * 5f);
    }

    private void HandleJump()
    {
        if (isGroundedBuffered && input.JumpPressed)
            verticalVelocity = Mathf.Sqrt(1.2f * -2f * -9.81f);
    }

    private void ApplyGravity()
    {
        verticalVelocity += -9.81f * Time.deltaTime;
    }

    private void ApplyMovement()
    {
        Vector3 velocity = smoothedMove * moveSpeed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
}
