using UnityEngine;
using static Math;

public class SimpleFpsController : MonoBehaviour
{
    public float lookSpeed = 0.25f, moveSpeed = 2, jump = 10, gravity = 9.81f, height = 1.8f;
    private float pitch, yaw, velocity;

    public float scale, vel;

    private void Update()
    {
        pitch += Input.GetAxisRaw("Mouse Y") * lookSpeed;
        yaw += Input.GetAxisRaw("Mouse X") * lookSpeed;
        var phi = Radians(pitch);
        var theta = Radians(yaw);

        var sinTheta = Sin(theta);
        var cosTheta = Cos(theta);
        var sinPhi = Sin(phi);
        var cosPhi = Cos(phi);
        transform.rotation = Quaternion.LookRotation(new(cosPhi * sinTheta, sinPhi, cosPhi * cosTheta), new(-sinPhi * sinTheta, cosPhi, -sinPhi * cosTheta));

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        // Crazy square to circle mapping
        var tangent = horizontal == 0 ? 0 : Abs(vertical / horizontal);
        var theta1 = Abs(QuarterPi - Atan(tangent));
        scale = Remap(theta1, 0, QuarterPi, Mathf.Sqrt(0.5f), 1);
        var movement = moveSpeed * scale * new Vector3(horizontal * cosTheta + vertical * sinTheta, 0f, -horizontal * sinTheta + vertical * cosTheta);

        // Jump
        if (transform.position.y > height)
            velocity -= gravity * Time.deltaTime;
        else if (Input.GetButtonDown("Jump"))
            velocity = jump;
        else
            velocity = (height - transform.position.y) / Time.deltaTime;

        movement.y = velocity;

        vel = movement.magnitude;

        transform.position += movement * Time.deltaTime;
    }
}
