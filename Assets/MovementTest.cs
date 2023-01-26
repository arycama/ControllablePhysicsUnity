using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private Camera camera;
    [SerializeField] private Rigidbody rigidbody;

    private float target;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                target = hit.point.x;
            }
        }
    }

    private void FixedUpdate()
    {
        // Need to find correct value for this
        var t = Time.fixedDeltaTime;
        var position = rigidbody.position.x;

        var distance = Mathf.Abs(target - position);
        var targetVelocity = Mathf.Sqrt(2f * acceleration * distance) * Mathf.Sign(target - position);
        var velocityDelta = targetVelocity - rigidbody.velocity.x;
        var force = Mathf.Clamp(velocityDelta / t, -acceleration, acceleration);

        rigidbody.AddForce(force, 0f, 0f, ForceMode.Acceleration);
    }
}
