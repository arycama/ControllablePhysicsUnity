using UnityEngine;
using static Math;

public class FpsPhysicsControllerSimpleVelocity : MonoBehaviour
{
    public float
        moveSpeed = 5f,
        jumpHeight = 1f,
        acceleration = 5f;

    private bool jump;
    public Vector3 targetVelocity, force, clampedForce;
    public float accel, speed;

    private Camera camera;
    private Rigidbody rigidbody;

    private bool toggle;

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
        var theta = Radians(camera.transform.eulerAngles.y);
        SinCos(theta, out var sinTheta, out var cosTheta);

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        jump = rigidbody.position.y <= 0 && Input.GetButtonDown("Jump");

        // Crazy square to circle mapping
        var tangent = horizontal == 0 ? 0 : Abs(vertical / horizontal);
        var theta1 = Abs(QuarterPi - Atan(tangent));
        var scale = Remap(theta1, 0, QuarterPi, Mathf.Sqrt(0.5f), 1);
        targetVelocity = moveSpeed * scale * new Vector3(horizontal * cosTheta + vertical * sinTheta, 0f, -horizontal * sinTheta + vertical * cosTheta);

        rigidbody.drag = acceleration / (moveSpeed + acceleration * Time.fixedDeltaTime);

        if (Input.GetKeyDown(KeyCode.T))
            toggle = !toggle;
    }

    private void FixedUpdate()
    {
        var t = Time.fixedDeltaTime;
        var k = rigidbody.drag;
        var v0 = rigidbody.velocity;
        var a = Vector3.ClampMagnitude((targetVelocity - v0 * (1 - k * t)).Y0() / (t * (1 - k * t)), acceleration);

        var nextSpeed = (1 - k * t) * (a * t + v0);

        // Amount of force needed to achieve nextSpeed in next frame, as well as overcoming the additional drag to ensure nextspeed is actually reached
        force = (nextSpeed - v0 * (1 - k * t)).Y0() / (t * (1 - k * t));

        //force = (targetVelocity - rigidbody.velocity * (1 - rigidbody.drag * Time.fixedDeltaTime)).Y0() / (Time.fixedDeltaTime * (1 - rigidbody.drag * Time.fixedDeltaTime));
        clampedForce = force;// Vector3.ClampMagnitude(force, acceleration);

        if (toggle)
        {
            force = (targetVelocity - rigidbody.velocity * (1 - rigidbody.drag * Time.fixedDeltaTime)).Y0() / (Time.fixedDeltaTime * (1 - rigidbody.drag * Time.fixedDeltaTime));
            clampedForce = Vector3.ClampMagnitude(force, acceleration);
        }

        if (jump)
            clampedForce.y = Sqrt(2 * jumpHeight * Abs(Physics.gravity.y)) / Time.fixedDeltaTime;

        var prevSpeed = speed;
        speed = rigidbody.velocity.magnitude;
        accel = (speed - prevSpeed) / Time.fixedDeltaTime;

        rigidbody.AddForce(clampedForce, ForceMode.Acceleration);
    }
}
