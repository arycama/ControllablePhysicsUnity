using UnityEngine;
using static UnityEngine.Mathf;

public class MovementTest3D : MonoBehaviour
{
    public float acceleration = 1;

    [SerializeField] private Camera camera;

    public Vector3 target;
    public Vector3 velocity;

    public static float Sq(float x) => x * x;

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                target = hit.point;
            }
        }

        var current = transform.position;

        var delta = target - current;
        var direction = delta.normalized;
        var distance = delta.magnitude;

        // Sadness
        var a = acceleration;
        var t = Time.deltaTime;
        var x = distance;

        var targetVelocity = (Sqrt(Sq(a * t) + 8 * a * x) - a * t) * 0.5f;

        var targetAcceleration = (direction * targetVelocity - velocity) / Time.deltaTime;
        var clampedAcceleration = Vector3.ClampMagnitude(targetAcceleration, acceleration);

        velocity += clampedAcceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }
}
