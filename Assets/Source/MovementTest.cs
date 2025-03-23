using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [SerializeField] private float acceleration = 15;
    [SerializeField] private float maxTorque = 15;
    [SerializeField] private float height = 0;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Camera camera;

    private Vector3 targetPosition;
    private Vector3 targetForward;

    private void OnEnable()
    {
        targetPosition = transform.position;
        targetForward = transform.forward;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            targetPosition.x = hit.point.x;
            targetPosition.z = hit.point.z;
            targetForward = Vector3.Normalize(new Vector3(hit.point.x, height, hit.point.z) - rigidbody.position);
        }
    }

    public float accel;


    private void FixedUpdate()
    {
        targetPosition.y = height;

        var velocity = rigidbody.velocity;
        if (rigidbody.useGravity)
            velocity += Physics.gravity * Time.fixedDeltaTime;

        var force = PhysicsExtensions.AccelerateToTarget(velocity, rigidbody.position, targetPosition, acceleration, Time.fixedDeltaTime);

        accel = force.magnitude;

        rigidbody.AddForce(force, ForceMode.Acceleration);

        var targetRotation = Quaternion.LookRotation(targetForward);

        var rotation = rigidbody.rotation;
        var deltaRotation = targetRotation * Quaternion.Inverse(rotation);
        deltaRotation.ToAngleAxis(out var angle, out var axis);

        if (!float.IsInfinity(axis.x))
        {
            //if (angle > 180f)
            //    angle -= 360f;

            angle = Quaternion.Angle(rotation, targetRotation);

            angle *= Mathf.Deg2Rad;
            //angle = Mathf.Repeat(angle, 2.0f * Mathf.PI);

            var speed = rigidbody.angularVelocity.magnitude;
            MathExtensions.SolveQuadratic(maxTorque, speed, (0.5f * speed * speed - angle * maxTorque) / (4 * maxTorque), out var timeToHalfPointA, out var timeToHalfPointB);

            //var timeToHalfPoint = angle >= 0f ? timeToHalfPointB : timeToHalfPointA;
            var timeToHalfPoint = timeToHalfPointB;

            float targetSpeed;
            if (Time.fixedDeltaTime < timeToHalfPoint)
                targetSpeed = PhysicsExtensions.VelocityAtTime(speed, maxTorque, Time.fixedDeltaTime);
            else
                targetSpeed = PhysicsExtensions.VelocityAtTime(PhysicsExtensions.VelocityAtTime(speed, maxTorque, timeToHalfPoint), -maxTorque, Time.fixedDeltaTime - timeToHalfPoint);

            var targetAngularVelocity = targetSpeed * axis.normalized;
            var torque = (targetAngularVelocity - rigidbody.angularVelocity) / Time.fixedDeltaTime;
            var clampedTorque = Vector3.ClampMagnitude(torque, maxTorque);

            rigidbody.AddTorque(clampedTorque, ForceMode.Acceleration);
        }
    }
}
