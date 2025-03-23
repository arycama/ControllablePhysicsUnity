using UnityEngine;

public class HovershipEngine : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 50f;
    [SerializeField] private float elevationSpeed = 50f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float pitchSpeed = 30f;
    [SerializeField, Range(0f, 90f)] private float strafePitch = 15f;
    [SerializeField, Range(0f, 90f)] private float boostPitch = 60f;
    [SerializeField, Range(0f, 90f)] private float strafeRoll = 15f;
    [SerializeField, Range(0f, 90f)] private float boostRoll = 75f;

    [Header("References")]
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform camera;

    private void FixedUpdate()
    {
        var moveInput = new Vector2(Input.GetKey(KeyCode.D) ? 1f : (Input.GetKey(KeyCode.A) ? -1f : 0f), Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f));
        var elevationInput = Input.GetKey(KeyCode.Space) ? 1f : (Input.GetKey(KeyCode.LeftControl) ? -1f : 0f);
        var boostInput = Input.GetKey(KeyCode.LeftShift) ? 1f : 0f;

        // Some constants
        var angularDrag = rigidbody.angularDrag;
        var timeStep = Time.fixedDeltaTime;
        var maxAngularSpeed = rotationSpeed * Mathf.Deg2Rad * angularDrag / (1f - angularDrag * timeStep);

        // Our target velocity
        var cameraRight = Vector3.ProjectOnPlane(camera.right, Vector3.up).normalized;
        var cameraForward = Vector3.ProjectOnPlane(camera.forward, Vector3.up).normalized;

        var targetVelocity = boostInput * movementSpeed * transform.forward;

        targetVelocity.x += (cameraRight.x * moveInput.x + cameraForward.x * moveInput.y) * elevationSpeed * Mathf.Clamp01(1f - boostInput);
        targetVelocity.y += elevationInput * elevationSpeed * Mathf.Clamp01(1f - boostInput);
        targetVelocity.z += (cameraRight.z * moveInput.x + cameraForward.z * moveInput.y) * elevationSpeed * Mathf.Clamp01(1f - boostInput);

        // Target rotation
        var projectedForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        var currentYaw = Vector3.SignedAngle(Vector3.forward, projectedForward.normalized, Vector3.up);
        var targetYaw = currentYaw + moveInput.x * maxAngularSpeed * Mathf.Rad2Deg * Time.fixedDeltaTime * Mathf.Clamp01(boostInput);
        var finalYaw = Mathf.Repeat(targetYaw - 180f, 360f) - 180f;

        var yawRotation = Quaternion.AngleAxis(finalYaw, Vector3.up);

        var targetRight = yawRotation * Vector3.right;
        var currentPitch = Vector3.SignedAngle(Vector3.up, transform.forward, targetRight) - 90;
        var targetPitch = currentPitch + moveInput.y * pitchSpeed * Time.fixedDeltaTime * boostInput;
        var finalPitch = Mathf.Repeat(targetPitch - 180f, 360f) - 180f;
        finalPitch = Mathf.Clamp(finalPitch, -boostPitch, boostPitch);
        finalPitch = Mathf.LerpAngle(0f, finalPitch, Mathf.Abs(moveInput.y));// pitchSpeed * (1f - Mathf.Abs(moveInput.y)) * Time.fixedDeltaTime);

        var flightPitchRotation = Quaternion.AngleAxis(finalPitch, targetRight);
        var flightForward = flightPitchRotation * (yawRotation * Vector3.forward);

        var hoverPitchRotation = Quaternion.AngleAxis(moveInput.y * strafePitch, targetRight);
        var hoverForward = hoverPitchRotation * Vector3.Lerp(projectedForward, cameraForward, moveInput.magnitude);
        var targetForward = Vector3.Lerp(hoverForward, flightForward, boostInput);

        var rollRotation = Quaternion.AngleAxis(-moveInput.x * Mathf.Lerp(strafeRoll, boostRoll, boostInput), projectedForward);
        var targetUp = rollRotation * Vector3.up;

        var targetRotation = Quaternion.LookRotation(targetForward, targetUp);

        // Compensate for change in velocity next frame due to gravity and drag
        var velocity = rigidbody.velocity;
        var drag = rigidbody.drag;
        var futureVelocity = velocity - drag * timeStep * velocity;
        var maxForce = movementSpeed * drag / (1f - drag * timeStep);

        if (rigidbody.useGravity)
        {
            var gravity = Physics.gravity;
            futureVelocity += gravity * timeStep;
        }

        var force = Vector3.ClampMagnitude((targetVelocity - futureVelocity) / (timeStep * (1f - drag * timeStep)), movementSpeed);

        rigidbody.AddForce(force, ForceMode.Acceleration);

        // Torque/rotation
        var rotation = rigidbody.rotation;
        var angularVelocity = rigidbody.angularVelocity;
        var spin = new Quaternion(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0f) * rotation;
        var futureRotation = new Quaternion(rotation.x + 0.5f * spin.x * timeStep, rotation.y + 0.5f * spin.y * timeStep, rotation.z + 0.5f * spin.z * timeStep, rotation.w + 0.5f * spin.w * timeStep).normalized;

        var deltaRotation = targetRotation * Quaternion.Inverse(futureRotation);
        deltaRotation.ToAngleAxis(out var angle, out var axis);

        // We get an infinite axis in the event that our rotation is already aligned.
        if (!float.IsInfinity(axis.x))
        {
            if (angle > 180f)
                angle -= 360f;

            angle *= Mathf.Deg2Rad;

            // Compensate for change in velocity next frame due to gravity and drag
            var futureAngularVelocity = angularVelocity - angularDrag * timeStep * angularVelocity;
            var targetAngularVelocity = (angle >= 0f ? Mathf.Sqrt(2f * maxAngularSpeed * angle) : -Mathf.Sqrt(-2f * maxAngularSpeed * angle)) * axis.normalized;

            var torque = (targetAngularVelocity - futureAngularVelocity) / (timeStep * (1f - angularDrag * timeStep));
            var clampedTorque = Vector3.ClampMagnitude(torque, maxAngularSpeed);

            rigidbody.AddTorque(clampedTorque, ForceMode.Acceleration);
        }
    }
}
