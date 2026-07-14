using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool BuildPressed { get; private set; }
    public bool BackPressed { get; private set; }
    public float ScrollInput { get; private set; }

    private InputAction move, look, jump, sprint, attack, interact, build, back, scroll;

    public void Initialize()
    {
        move = InputSystem.actions.FindAction("move");
        look = InputSystem.actions.FindAction("look");
        jump = InputSystem.actions.FindAction("jump");
        sprint = InputSystem.actions.FindAction("sprint");
        attack = InputSystem.actions.FindAction("Attack");
        interact = InputSystem.actions.FindAction("interact");
        build = InputSystem.actions.FindAction("BuildKey");
        back = InputSystem.actions.FindAction("Back");
        scroll = InputSystem.actions.FindAction("scroll");

        move.Enable(); look.Enable(); jump.Enable(); sprint.Enable();
        attack.Enable(); interact.Enable(); build.Enable(); back.Enable();
        scroll.Enable();
    }

    private void Update()
    {
        MoveInput = move.ReadValue<Vector2>();
        LookInput = look.ReadValue<Vector2>();
        JumpPressed = jump.WasPressedThisFrame();
        SprintHeld = sprint.IsPressed();
        AttackPressed = attack.WasPressedThisFrame();
        InteractPressed = interact.WasPressedThisFrame();
        BuildPressed = build.WasPressedThisFrame();
        BackPressed = back.WasPressedThisFrame();
        ScrollInput = scroll.ReadValue<float>();

        if (InteractPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                IInteractInterface interactable = hit.collider.GetComponent<IInteractInterface>();
                if (interactable != null)
                {
                    interactable.Interact(gameObject);
                }
            }
        }
    }

}
