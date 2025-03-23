using UnityEngine;
using static UnityEngine.Mathf;

public class AutoCameraFollow1 : MonoBehaviour
{
    [SerializeField] private Vector3 lookOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] private float distance = 15f;
    [SerializeField] private Transform target = null;
    [SerializeField] private float lookSpeed = 0.01f;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float zoomDamping = 1f;
    [SerializeField] private float rotateSpeed = 1;
    [SerializeField] private float power = 1f;

    private float autoYaw, yaw, pitch, currentDistance;

    private void OnEnable()
    {
        currentDistance = distance;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void LateUpdate()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        var mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var position = new Vector2(horizontal, vertical);
        var angle = DeltaAngle(90, Atan2(position.y, position.x) * Rad2Deg);

        var amount = position.magnitude;

        angle = Pow(Abs(angle), power) * Sign(angle);

        autoYaw = autoYaw + angle * amount * rotateSpeed * Time.deltaTime;

        autoYaw = Repeat(autoYaw, 360);

        yaw = Repeat(yaw - mouseX * lookSpeed, 360);
        pitch = Clamp(pitch + mouseY * lookSpeed, 0f, 180);

        var finalYaw = yaw + autoYaw;

        transform.position = target.position + SphericalCoordinates.SphericalToCartesian(finalYaw * Deg2Rad, pitch * Deg2Rad, distance);
        transform.LookAt(target.position + Quaternion.AngleAxis(-finalYaw, Vector3.up) * lookOffset);

        //transform.position = Vector3.Lerp(transform.position, target.position + target.rotation * offset, Time.deltaTime * positionSmoothing);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), Time.deltaTime * rotationSmoothing);
    }
}
