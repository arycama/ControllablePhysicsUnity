using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 lookOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] private float distance = 15f;
    [SerializeField] private Transform target = null;
    [SerializeField] private float lookSpeed = 0.01f;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float zoomDamping = 1f;

    private float theta, phi, currentDistance;

    private void OnEnable()
    {
        currentDistance = distance;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        var mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        //currentDistance -= mouseWheel * zoomSpeed;

        theta = Repeat(theta - mouseX * lookSpeed * Deg2Rad, 2f * PI);
        phi = Clamp(phi + mouseY * lookSpeed * Deg2Rad, 0f, PI);
    }

    private void LateUpdate()
    {
        transform.position = target.position + SphericalCoordinates.SphericalToCartesian(theta, phi, distance);
        transform.LookAt(target.position + lookOffset);

        //transform.position = Vector3.Lerp(transform.position, target.position + target.rotation * offset, Time.deltaTime * positionSmoothing);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), Time.deltaTime * rotationSmoothing);
    }
}
