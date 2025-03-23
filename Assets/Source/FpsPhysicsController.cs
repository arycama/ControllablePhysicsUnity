using UnityEngine;
using UnityEngine.Assertions.Must;
using static Math;

public class FpsPhysicsController : MonoBehaviour
{
    public float
        moveSpeed = 5f,
        strafeSpeed = 3f,
        sprintSpeed = 8f,
        acceleration = 5f;

    public float desiredSideSpeed, sideSpeed, sideA, sideForce, v0, a, force, desiredSpeed, nextSpeed, accel, speed;

    private Camera camera;
    private Rigidbody rigidbody;

    private void OnEnable()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        var theta = Radians(camera.transform.eulerAngles.y);
        SinCos(theta, out var sinTheta, out var cosTheta);

        // Crazy square to circle mapping
        var tangent = horizontal == 0 ? 0 : Abs(vertical / horizontal);
        var theta1 = Abs(QuarterPi - Atan(tangent));
        var scale = Remap(theta1, 0, QuarterPi, Mathf.Sqrt(0.5f), 1);
        var targetVelocity = moveSpeed * scale * new Vector3(horizontal * cosTheta + vertical * sinTheta, 0f, -horizontal * sinTheta + vertical * cosTheta);


        desiredSpeed = vertical * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed);
        desiredSideSpeed = horizontal * strafeSpeed;
        rigidbody.drag = acceleration / (sprintSpeed + acceleration * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        var t = Time.fixedDeltaTime;
        var k = rigidbody.drag;

        var theta = Radians(camera.transform.eulerAngles.y);
        SinCos(theta, out var sinTheta, out var cosTheta);

        var right = new Vector3(cosTheta, 0f, -sinTheta);
        var fwd = new Vector3(sinTheta, 0f, cosTheta);

        sideSpeed = Vector3.Dot(rigidbody.velocity, right);
        sideA = Clamp((desiredSideSpeed - sideSpeed * (1 - k * t)) / (t * (1 - k * t)), -acceleration, acceleration);
        var sideNextSpeed = (1 - k * t) * (sideA * t + sideSpeed);
        sideForce = (sideNextSpeed - sideSpeed * (1 - k * t)) / (t * (1 - k * t));

        var remainingAcceleration = acceleration - Abs(sideForce);
        v0 = Vector3.Dot(rigidbody.velocity, fwd);
        a = Clamp((desiredSpeed - v0 * (1 - k * t)) / (t * (1 - k * t)), -remainingAcceleration, remainingAcceleration);
        nextSpeed = (1 - k * t) * (a * t + v0);
        force = (nextSpeed - v0 * (1 - k * t)) / (t * (1 - k * t));

        rigidbody.AddForce(fwd * force + right * sideForce, ForceMode.Acceleration);

        var prevSpeed = speed;
        speed = rigidbody.velocity.magnitude;
        accel = (speed - prevSpeed) / t;

    }
}
