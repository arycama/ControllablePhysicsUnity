using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Math;

public class FpsCameraLook : MonoBehaviour
{
    public float speed = 5f;

    private float pitch, yaw;

    private void Update()
    {
        pitch += Input.GetAxisRaw("Mouse Y") * speed;
        yaw += Input.GetAxisRaw("Mouse X") * speed;
        var phi = Radians(pitch);
        var theta = Radians(yaw);

        SinCos(theta, out var sinTheta, out var cosTheta);
        SinCos(phi, out var sinPhi, out var cosPhi);
        transform.rotation = Quaternion.LookRotation(new(cosPhi * sinTheta, sinPhi, cosPhi * cosTheta), new(-sinPhi * sinTheta, cosPhi, -sinPhi * cosTheta));
    }
}
