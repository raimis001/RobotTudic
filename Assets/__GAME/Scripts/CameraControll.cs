using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraControll : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    //[SerializeField] private CinemachineVirtualCameraBase virtualCamera;

    [Header("Speeds")]
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float rotateSpeed = 120f;
    [SerializeField] private float pitchSpeed = 120f;
    [SerializeField] private float zoomSpeed = 10f;

    [Header("Zoom Limits")]
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 50f;

    [Header("Pitch Limits")]
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;

    // tracked pitch in degrees (signed -180..180) to avoid Euler wrap issues
    private float currentPitch;

    private void Update()
    {
        if (target == null) return;

        Pan();
        Rotate();
        Zoom();
    }

    private void Start()
    {
        if (target == null) return;

        // initialize tracked pitch from target's local X euler (convert to signed)
        currentPitch = target.localEulerAngles.x;
        if (currentPitch > 180f) currentPitch -= 360f;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        Vector3 e = target.localEulerAngles;
        e.x = currentPitch;
        target.localEulerAngles = e;
    }

    private void OnValidate()
    {
        if (minPitch > maxPitch)
        {
            float tmp = minPitch;
            minPitch = maxPitch;
            maxPitch = tmp;
        }
    }

    private void Zoom()
    {
        if (target == null) return;

        // get zoom input: prefer configured InputAction if provided/enabled, otherwise use mouse wheel
        float zoomInput = 0f;
        zoomInput = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(zoomInput) < Mathf.Epsilon) return;

        // Move the target up/down with the scroll input. Positive scroll -> move up, negative -> move down.
        float deltaY = -zoomInput * zoomSpeed * Time.deltaTime;
        Vector3 pos = target.position;
        pos.y = Mathf.Clamp(pos.y + deltaY, minZoom, maxZoom);
        target.position = pos;
    }

    void Rotate()
    {
        if (!Mouse.current.rightButton.isPressed)
            return;

        Vector2 rotateInput = Mouse.current.delta.ReadValue();

        // Horizontal rotation around world's Y so the camera orbits the target
        float yaw = rotateInput.x * rotateSpeed * Time.deltaTime;
        target.Rotate(Vector3.up, yaw, Space.World);

        // Local pitch on the target, clamped to min/max
        float pitchDelta = -rotateInput.y * pitchSpeed * Time.deltaTime;

        currentPitch = Mathf.Clamp(currentPitch + pitchDelta, minPitch, maxPitch);

        Vector3 localEuler = target.localEulerAngles;
        localEuler.x = currentPitch;
        target.localEulerAngles = localEuler;
    }

    void Pan()
    {
        if (!Mouse.current.middleButton.isPressed)
            return;

        Vector2 panInput = Mouse.current.delta.ReadValue();

        Vector3 camForward = transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        Vector3 camRight = transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        // Invert input so dragging the mouse moves the world under the cursor
        Vector3 panDelta = (camRight * -panInput.x + camForward * -panInput.y) * panSpeed * Time.deltaTime;
        target.position += panDelta;
    }
}