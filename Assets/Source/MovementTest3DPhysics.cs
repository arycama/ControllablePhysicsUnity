using UnityEngine;
using static UnityEngine.Mathf;

public class MovementTest3DPhysics : MonoBehaviour
{
    public float acceleration = 1;
    public float torque = 1;
    public LayerMask layers = ~0;

    public Camera camera;
    public Rigidbody rigidbody;

    public Vector3 target;

    public static float Sq(float x) => x * x;

    private void Update()
    {
       // if (Input.GetMouseButtonDown(0))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layers))
            {
                target = hit.point;
            }
        }
    }

    private void FixedUpdate()
    {
        var delta = target - rigidbody.position;
        var distance = delta.magnitude;
        if (distance > 0)
        {
            var targetVelocity = (Sqrt(Sq(acceleration * Time.fixedDeltaTime) + 8 * acceleration * distance) - acceleration * Time.fixedDeltaTime) * 0.5f;
            var direction = delta / distance;

            var currentVelocity = rigidbody.velocity;
            if (rigidbody.useGravity)
                currentVelocity += Physics.gravity * Time.fixedDeltaTime;

            var targetAcceleration = (direction * targetVelocity - currentVelocity) / Time.fixedDeltaTime;
            var clampedAcceleration = Vector3.ClampMagnitude(targetAcceleration, acceleration);

            rigidbody.AddForce(clampedAcceleration, ForceMode.Acceleration);
        }


    }
}