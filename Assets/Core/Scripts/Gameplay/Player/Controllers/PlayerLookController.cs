using UnityEngine;

public class PlayerLookController : MonoBehaviour
{
    private PlayerInputHandler input;
    public Camera cam;

    private float xRot;
    public bool LookEnabled { get; set; } = false;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        cam = GetComponentInChildren<Camera>();
    }

    public void Tick()
    {
        if (!LookEnabled) return;

        float mouseX = input.LookInput.x * 0.05f;
        float mouseY = input.LookInput.y * 0.05f;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -70f, 85f);

        cam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
