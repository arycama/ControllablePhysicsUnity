using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [SerializeField] private float maxAcceleration = 1f;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Camera camera;

    private Vector3 targetPosition;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hit))
        {
            targetPosition = hit.point;
        }
    }

    private void FixedUpdate()
    {
        var targetDistance = Vector3.Distance(targetPosition, rigidbody.position);
        if (targetDistance < 1e-3f)
            return;

        var targetDirection = Vector3.Normalize(targetPosition - rigidbody.position);
        var targetVelocity = Mathf.Sqrt(2.0f * maxAcceleration * targetDistance) * targetDirection;

        var velocityDelta = (targetVelocity - rigidbody.velocity) / Time.fixedDeltaTime;
        var force = Vector3.ClampMagnitude(velocityDelta, maxAcceleration);

        rigidbody.AddForce(force, ForceMode.Acceleration);

    }
}
