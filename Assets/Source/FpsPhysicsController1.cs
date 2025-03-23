using UnityEngine;
using static Math;

public class FpsPhysicsController1 : MonoBehaviour
{
    public float
        moveSpeed = 5f,
        acceleration = 5f;

    public float a, v0, force, desiredSpeed, nextSpeed, speed, accel;

    private Camera camera;
    private Rigidbody rigidbody;

    private void OnEnable()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var theta = Radians(camera.transform.eulerAngles.y);
        SinCos(theta, out var sinTheta, out var cosTheta);

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        desiredSpeed = vertical * moveSpeed;
        rigidbody.drag = acceleration / (moveSpeed + acceleration * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        var t = Time.fixedDeltaTime;
        var k = rigidbody.drag;
        v0 = rigidbody.velocity.z;
        a = Clamp((desiredSpeed - v0 * (1 - k * t)) / (t * (1 - k * t)), -acceleration, acceleration);

        nextSpeed = (1 - k * t) * (a * t + v0);

        // Amount of force needed to achieve nextSpeed in next frame, as well as overcoming the additional drag to ensure nextspeed is actually reached
        force = (nextSpeed - v0 * (1 - k * t)) / (t * (1 - k * t));

        var prevSpeed = speed;
        speed = rigidbody.velocity.magnitude;
        accel = (speed - prevSpeed) / t;

        rigidbody.AddForce(Vector3.forward * force, ForceMode.Acceleration);
    }
}
