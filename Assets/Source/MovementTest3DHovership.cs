using UnityEngine;

public class MovementTest3DHovership : MonoBehaviour
{
    public float acceleration = 32;
    public float torque = 1;

    public Camera camera;
    public Rigidbody rigidbody;

    public Vector3 moveVector;
    public float accel;
    public float targetSpeed;

    private void Update()
    {
        Vector2 moveInput;
        moveInput.x = Input.GetKey(KeyCode.D) ? 1f : (Input.GetKey(KeyCode.A) ? -1f : 0f);
        moveInput.y = Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f);

        var targetFwd = Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized;
        var targetRgt = Vector3.ProjectOnPlane(camera.transform.right, Vector3.up).normalized;

        moveVector = targetRgt * moveInput.x + targetFwd * moveInput.y;
    }

    private void FixedUpdate()
    {
        //var targetVelocity = (targetRgt * moveInput.x + targetFwd * moveInput.y) * acceleration;


        //var delta = target - rigidbody.position;
        //var distance = delta.magnitude;

        //var targetVelocity = (Mathf.Sqrt(Sq(acceleration * Time.fixedDeltaTime) + 8 * acceleration * distance) - acceleration * Time.fixedDeltaTime) * 0.5f;
        //var direction = delta / distance;
        //var targetAcceleration = (targetVelocity - rigidbody.velocity) / Time.fixedDeltaTime;
        //var clampedAcceleration = Vector3.ClampMagnitude(targetAcceleration, acceleration);

        //rigidbody.AddForce(clampedAcceleration, ForceMode.Acceleration);

        accel = (rigidbody.velocity.magnitude - accel);// / Time.fixedDeltaTime;

         targetSpeed = PhysicsExtensions.DisplacementFromAccelerationAndVelocity(acceleration, rigidbody.velocity.magnitude / Time.deltaTime);

        var target = rigidbody.position + moveVector * targetSpeed;

        var distance = Vector3.Distance(rigidbody.position, target);
        var targetVelocity = PhysicsExtensions.VelocityFromAccelerationAndDisplacement(acceleration, distance);
        var direction = Vector3.Normalize(target - rigidbody.position);

        var currentVelocity = rigidbody.velocity;
        if (rigidbody.useGravity)
            currentVelocity += Physics.gravity * Time.fixedDeltaTime;

        var targetAcceleration = (direction * targetVelocity - currentVelocity) / Time.fixedDeltaTime;
        var clampedAcceleration = Vector3.ClampMagnitude(targetAcceleration, acceleration);

        rigidbody.AddForce(clampedAcceleration, ForceMode.Acceleration);
    }
}
