using UnityEngine;
using static Math;

public class MovementTest : MonoBehaviour
{
    [SerializeField] private float acceleration = 15;
    [SerializeField] private float maxTorque = 15;
    [SerializeField] private float height = 0;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Camera camera;

    public Float3 targetPosition;
    private Float3 targetForward;

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
            targetForward = Normalize(Float3(hit.point.x, height, hit.point.z) - (Float3)rigidbody.position);
        }
    }

    public float accel, speed;

    // Calculates an acceleration force to reach a target without overshooting
    public Float3 AccelerateToTarget1(Float3 velocity, Float3 position, Float3 target, float acceleration, float deltaTime)
    {
        var direction = Normalize(target - position);
        speed = Magnitude(velocity);// Dot(velocity, direction);

        var delta = Distance(target, position);
        var slowdownDistance = (2 * acceleration * delta - Square(speed)) / (4 * acceleration);

        var slowdownTime0 = (-speed + Sqrt(Square(speed) + 2 * acceleration * slowdownDistance)) / acceleration;
        var slowdownTime1 = (speed + Sqrt(0.5f * Square(speed) - acceleration * delta)) / acceleration;

        var v0 = +acceleration * slowdownTime0 - acceleration * Abs(deltaTime - slowdownTime0);
        var v1 = -acceleration * slowdownTime1 + acceleration * Abs(deltaTime - slowdownTime1);

        var v2 = slowdownTime1 < 0 ? v0 : v1;
        var v3 = slowdownTime0 < 0 ? v1 : v0;
        var v4 = delta < 0 ? v2 : v3;

        return direction * v4 / deltaTime;


        //var delta = target - position;
        //var slowdownDistance = (2 * acceleration * delta - Square(velocity)) / (4 * acceleration);

        //var slowdownTime0 = (-velocity + Sqrt(Square(velocity) + 2 * acceleration * slowdownDistance)) / acceleration;
        //var slowdownTime1 = (velocity + Sqrt(0.5f * Square(velocity) - acceleration * delta)) / acceleration;

        //var v0 = +acceleration * slowdownTime0 - acceleration * Abs(deltaTime - slowdownTime0);
        //var v1 = -acceleration * slowdownTime1 + acceleration * Abs(deltaTime - slowdownTime1);

        //var v2 = Select(slowdownTime1 < 0, v0, v1);
        //var v3 = Select(slowdownTime0 < 0, v1, v0);
        //var v4 = Select(delta < 0, v2, v3);

        //return v4 / deltaTime;
    }

    private void FixedUpdate()
    {
        targetPosition.y = height;

        var velocity = rigidbody.velocity;
        if (rigidbody.useGravity)
            velocity += Physics.gravity * Time.fixedDeltaTime;

        var force = AccelerateToTarget1(velocity, rigidbody.position, targetPosition, acceleration, Time.fixedDeltaTime);

        accel = Magnitude(force);

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
            SolveQuadratic(maxTorque, speed, (0.5f * speed * speed - angle * maxTorque) / (4 * maxTorque), out var timeToHalfPointA, out var timeToHalfPointB);

            //var timeToHalfPoint = angle >= 0f ? timeToHalfPointB : timeToHalfPointA;
            var timeToHalfPoint = timeToHalfPointB;

            float targetSpeed;
            if (Time.fixedDeltaTime < timeToHalfPoint)
                targetSpeed = PhysicsExtensions.VelocityAtTime(speed, maxTorque, Time.fixedDeltaTime);
            else
                targetSpeed = PhysicsExtensions.VelocityAtTime(PhysicsExtensions.VelocityAtTime(speed, maxTorque, timeToHalfPoint), -maxTorque, Time.fixedDeltaTime - timeToHalfPoint);

            var targetAngularVelocity = targetSpeed * axis.normalized;
            var torque = (targetAngularVelocity - rigidbody.angularVelocity) / Time.fixedDeltaTime;
            var clampedTorque = ClampMagnitude(torque, maxTorque);

            rigidbody.AddTorque(clampedTorque, ForceMode.Acceleration);
        }
    }
}
