using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float positionSmoothing = 0.5f;
    [SerializeField] private float rotationSmoothing = 0.5f;
    [SerializeField] private Transform target = null;

    [SerializeField] private float lookSpeed = 0.01f;

    private float theta, phi;

    private void LateUpdate()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        theta = Mathf.Repeat(theta - mouseX * lookSpeed * Mathf.Deg2Rad, 2f * Mathf.PI);
        phi = Mathf.Clamp(phi - mouseY * lookSpeed, 0f, Mathf.PI);

        var sinTheta = Sin(theta);
        var cosTheta = Cos(theta);
        var sinPhi = Sin(phi);
        var cosPhi = Cos(phi);

        var up = new Vector3(cosPhi * sinTheta, sinPhi, -cosPhi * cosTheta);
        var fwd = new Vector3(-sinPhi * sinTheta, cosPhi, sinPhi * cosTheta);

        //transform.position = Vector3.Lerp(transform.position, target.position + target.rotation * offset, Time.deltaTime * positionSmoothing);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), Time.deltaTime * rotationSmoothing);
    }
}
