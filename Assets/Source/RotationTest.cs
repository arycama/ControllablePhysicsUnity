using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    [SerializeField] private float acceleration = 15;
    [SerializeField] private float torque = 15;
    [SerializeField] private float height = 5;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Camera camera;

    public Vector3 targetPosition;

    private void OnEnable()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            targetPosition.x = hit.point.x;
            targetPosition.y = height;
            targetPosition.z = hit.point.z;
        }
    }

    private void FixedUpdate()
    {
        var currentVelocity = rigidbody.velocity;
        if (rigidbody.useGravity)
            currentVelocity += Physics.gravity * Time.fixedDeltaTime;

        var force = PhysicsExtensions.AccelerateToTarget(currentVelocity, rigidbody.position, targetPosition, acceleration);
        rigidbody.AddForce(force, ForceMode.Acceleration);
    }
}
