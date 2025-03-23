using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSolver : MonoBehaviour
{
    public float maxSpeed = 250;

    public float targetAngle;

    public Rigidbody rigidbody;

    private void FixedUpdate()
    {
        var cosAngle = Mathf.Cos(targetAngle * Mathf.Deg2Rad);
        var sinAngle = Mathf.Sin(targetAngle * Mathf.Deg2Rad);

        var angularVelocity = rigidbody.angularVelocity;
        var rotation = rigidbody.rotation;
        var timeStep = Time.fixedDeltaTime;

        var spin = new Quaternion(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0f) * rotation;
        var futureRotation = new Quaternion(rotation.x + 0.5f * spin.x * timeStep, rotation.y + 0.5f * spin.y * timeStep, rotation.z + 0.5f * spin.z * timeStep, rotation.w + 0.5f * spin.w * timeStep).normalized;

        var targetForward = new Vector3(cosAngle, 0f, sinAngle);
        var targetRotation = Quaternion.LookRotation(targetForward);

        var delta = targetRotation * Quaternion.Inverse(futureRotation);
        delta.ToAngleAxis(out var angle, out var axis);
        angle *= Mathf.Deg2Rad;

        // Compensate for change in velocity next frame due to gravity and drag
        var futureVelocity = angularVelocity - rigidbody.angularDrag * angularVelocity * timeStep;
        var maxAcceleration = maxSpeed * Mathf.Deg2Rad * rigidbody.angularDrag / (1f - rigidbody.angularDrag * timeStep);
        var targetVelocity = Mathf.Sqrt(2f * maxAcceleration * angle) * axis.normalized;

        var torque = (targetVelocity - futureVelocity) / (timeStep * (1f - rigidbody.angularDrag * timeStep));
        var clampedTorque = Vector3.ClampMagnitude(torque, maxAcceleration);

        rigidbody.AddTorque(clampedTorque, ForceMode.Acceleration);
    }
}
