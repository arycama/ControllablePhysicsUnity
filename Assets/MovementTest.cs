using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [SerializeField] private float maxAcceleration = 1f;
    [SerializeField] private int iterations = 3;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Camera camera;

    private Vector3 targetPosition;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            targetPosition = hit.point;
        }
    }

    private void FixedUpdate()
    {
        var futurePosition = rigidbody.position + rigidbody.velocity * Time.fixedDeltaTime;
        var futureVelocity = rigidbody.velocity;
        var force = Vector3.zero;

        for (var i = 0; i < iterations; i++)
        {
            var targetDistance = Vector3.Distance(targetPosition, futurePosition);
            if (targetDistance < 1e-3f)
                break;

            var targetDirection = Vector3.Normalize(targetPosition - futurePosition);
            var targetVelocity = Mathf.Sqrt(2.0f * maxAcceleration * targetDistance) * targetDirection;

            var velocityDelta = (targetVelocity - futureVelocity) / Time.fixedDeltaTime;
            force = Vector3.ClampMagnitude(velocityDelta, maxAcceleration);

            futureVelocity = rigidbody.velocity + force * Time.fixedDeltaTime;
        }

        rigidbody.AddForce(force, ForceMode.Acceleration);

    }
}
