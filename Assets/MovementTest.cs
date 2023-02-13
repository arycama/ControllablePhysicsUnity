using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private bool test = false;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Camera camera;

    public Vector3 targetPosition;

    private void Update()
    {
        //if (!Input.GetMouseButtonDown(0))
        //    return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            targetPosition = hit.point;
        }
    }

    private void FixedUpdate()
    {
        var distance = Vector3.Distance(rigidbody.position, targetPosition);

        // Calculate the max velocity we can have and still reach the target with constant decel, based on distance
        var targetSpeed = Mathf.Sqrt(2f * acceleration * distance);

        // Assuming constant decel, calculate where we want to be next frame, instead of current
        targetSpeed = Mathf.Max(0f, targetSpeed - acceleration * Time.fixedDeltaTime));

        var direction = Vector3.Normalize(targetPosition - rigidbody.position);

        // Calculate acceleration required to get us to the desired velocity in a single frame
        var targetVelocity = direction * targetSpeed;
        var force = (targetVelocity - rigidbody.velocity) / Time.fixedDeltaTime;

        // Clamp to max accel
        var clampedForce = Vector3.ClampMagnitude(force, acceleration);

        rigidbody.AddForce(clampedForce, ForceMode.Acceleration);
    }
}