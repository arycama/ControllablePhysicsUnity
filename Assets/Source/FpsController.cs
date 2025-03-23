using UnityEngine;
using static Math;

public class FpsController : MonoBehaviour
{
    public float 
        lookSpeed = 0.25f, 
        moveSpeed = 5f,
        strafeSpeed = 2f,
        sprintSpeed = 8f,
        jump = 10, 
        gravity = 9.81f, 
        height = 1.8f, 
        acceleration = 5f;

    [Header("Lean")] public float leanHeight, leanAngle, leanSpeed;

    private float pitch, yaw;
    public Vector3 targetVelocity, velocity, force, clampedForce;
    public float speed, drag, accel;

    private void Update()
    {
        pitch += Input.GetAxisRaw("Mouse Y") * lookSpeed;
        yaw += Input.GetAxisRaw("Mouse X") * lookSpeed;
        var phi = Radians(pitch);
        var theta = Radians(yaw);

        SinCos(theta, out var sinTheta, out var cosTheta);
        SinCos(phi, out var sinPhi, out var cosPhi);
        transform.rotation = Quaternion.LookRotation(new(cosPhi * sinTheta, sinPhi, cosPhi * cosTheta), new(-sinPhi * sinTheta, cosPhi, -sinPhi * cosTheta));

        var horizontal = strafeSpeed * Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical") * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed);

        // Crazy square to circle mapping
        var tangent = horizontal == 0 ? 0 : Abs(vertical / horizontal);
        var theta1 = Abs(QuarterPi - Atan(tangent));
        var scale = Remap(theta1, 0, QuarterPi, Mathf.Sqrt(0.5f), 1);
        targetVelocity = scale * new Vector3(horizontal * cosTheta + vertical * sinTheta, 0f, -horizontal * sinTheta + vertical * cosTheta);

        //drag = acceleration / (sprintSpeed * sprintSpeed);
        drag = acceleration / sprintSpeed;

        force = (targetVelocity - velocity) / Time.deltaTime + drag * velocity;
        clampedForce = Vector3.ClampMagnitude(force, acceleration);

        velocity += clampedForce * Time.deltaTime;

        // Jump
        if (transform.position.y > height)
            velocity.y -= gravity * Time.deltaTime;
        else if (Input.GetButtonDown("Jump"))
            velocity.y = Sqrt(2 * jump * gravity);
        else
            velocity.y = (height - transform.position.y) / Time.deltaTime;

        velocity -= velocity * drag * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

        accel = (velocity.magnitude - speed) / Time.deltaTime;

        speed = velocity.magnitude;
    }
}
