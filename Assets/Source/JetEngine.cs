using UnityEngine;

public class JetEngine : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float accelerationTime = 2f;

    [SerializeField, Min(0f)]
    private float flightSpeed = 100f;

    [SerializeField, Min(0f)]
    private float maxHoverSpeed = 50f;

    [SerializeField, Min(0f), Tooltip("How quickly max hover speed is reached")]
    private float hoverAccelerationTime = 2f;

    [Header("Rotation")]
    [SerializeField, Range(0f, 90f)]
    private float maxPitch = 45f;

    [SerializeField, Range(0f, 90f)]
    private float maxRoll = 60f;

    [SerializeField, Min(0f)]
    private float yawSpeed = 90f;

    [SerializeField, Min(0f)]
    private float rotateTime = 2f;

    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private Transform camera;

    private float currentFlightSpeed;
    private Vector3 euler, currentHoverSpeed;

    private void Update()
    {
        var moveInput = new Vector2(Input.GetKey(KeyCode.D) ? 1f : (Input.GetKey(KeyCode.A) ? -1f : 0f), Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f));
        var elevationInput = Input.GetKey(KeyCode.Space) ? 1f : (Input.GetKey(KeyCode.LeftControl) ? -1f : 0f);
        var flightInput = Input.GetKey(KeyCode.LeftShift) ? 1f : 0f;

        euler.x = Mathf.LerpAngle(euler.x, moveInput.y * maxPitch, rotateTime * Time.deltaTime);
        euler.y += moveInput.x * yawSpeed * Time.deltaTime;
        euler.z = Mathf.LerpAngle(euler.z, -moveInput.x * maxRoll, rotateTime * Time.deltaTime);

        var flightRotation = Quaternion.Euler(euler);

        // Hover rotation, use current with a flat up
        var flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        var hoverRotation = Quaternion.LookRotation(flatForward);
        var finalRotation = Quaternion.Slerp(hoverRotation, flightRotation, flightInput);
        transform.rotation = finalRotation;

        // Position
        // Flight movement
        currentFlightSpeed = Mathf.Lerp(currentFlightSpeed, flightSpeed * flightInput, accelerationTime * Time.deltaTime);
        var flightMovement = transform.forward * currentFlightSpeed * Time.deltaTime;

        // Hover movement
        var camRgt = Vector3.ProjectOnPlane(camera.right, Vector3.up).normalized;
        var camFwd = Vector3.ProjectOnPlane(camera.forward, Vector3.up).normalized;

        Vector3 targetHover;
        targetHover.x = maxHoverSpeed * (camRgt.x * moveInput.x + camFwd.x * moveInput.y);
        targetHover.y = maxHoverSpeed * elevationInput;
        targetHover.z = maxHoverSpeed * (camRgt.z * moveInput.x + camFwd.z * moveInput.y);

        currentHoverSpeed = Vector3.Lerp(currentHoverSpeed, targetHover * (1f - flightInput), hoverAccelerationTime * Time.deltaTime);

        var finalMovement = currentHoverSpeed * Time.deltaTime + flightMovement;

        transform.position += finalMovement;
    }
}