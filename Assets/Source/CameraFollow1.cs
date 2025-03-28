using UnityEngine;

public class CameraFollow1 : MonoBehaviour
{
    [Header("Position")]
    [SerializeField]
    private float height = 1f;

    [SerializeField, Range(-1f, 1f)]
    private float pitchOffset = 0f;

    [SerializeField, Range(-1f, 1f)]
    private float yawOffset = 0f;

    [Header("Rotation")]
    [SerializeField]
    private float rotateSpeed = 0.005f;

    [SerializeField, Range(0, 0.995f)]
    private float maxPitch = 0.95f;

    [Header("Zoom")]
    [SerializeField]
    private float defaultZoom = 1f;

    [SerializeField]
    private float minZoom = 0.5f;

    [SerializeField]
    private float maxZoom = 5f;

    [SerializeField]
    private float zoomSmooth = 0.1f;

    [SerializeField]
    private float zoomSpeed = 0.25f;

    [SerializeField]
    private Transform target = null;

    private float currentZoom, targetZoom, zoomVelocity;
    private float currentYaw, currentPitch;

    private void OnEnable()
    {
        currentZoom = targetZoom = defaultZoom;
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;

        //var input = yawAction.action.ReadValue<Vector2>() * rotateSpeed * Mathf.Deg2Rad;

        var mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
        var mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;
        var zoom = Input.GetAxis("Mouse ScrollWheel");

        currentPitch = Mathf.Clamp(currentPitch + mouseY, -maxPitch, maxPitch);
        currentYaw = Mathf.Repeat(currentYaw + mouseX, 2 * Mathf.PI);

        targetZoom = Mathf.Clamp(targetZoom + zoom * zoomSpeed, minZoom, maxZoom);
        currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomVelocity, zoomSmooth);

        var cosPhi = Mathf.Cos(currentYaw);
        var sinPhi = Mathf.Sin(currentYaw);
        var sinTheta = Mathf.Sqrt(1f - currentPitch * currentPitch);
        var direction = new Vector3(sinTheta * sinPhi, currentPitch, sinTheta * cosPhi);

        var rotation = Quaternion.LookRotation(direction);

        var targetPosition = target.position + new Vector3(0f, height, 0f);
        transform.position = targetPosition + rotation * new Vector3(0f, 0f, -currentZoom);

        var offsetPitch = currentPitch + pitchOffset;
        var sinThetaOffset = Mathf.Sqrt(1f - Mathf.Pow(offsetPitch, 2f));
        var cosPhiOffset = Mathf.Cos(currentYaw + yawOffset / currentZoom);
        var sinPhiOffset = Mathf.Sin(currentYaw + yawOffset / currentZoom);
        var offsetDirection = new Vector3(sinThetaOffset * sinPhiOffset, offsetPitch, sinThetaOffset * cosPhiOffset);

        transform.rotation = Quaternion.LookRotation(offsetDirection);
    }
}