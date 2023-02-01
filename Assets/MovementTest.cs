using UnityEngine;
using static UnityEngine.Mathf;
using static UnityEngine.Vector3;

public class MovementTest : MonoBehaviour
{
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private bool test = false;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Camera camera;

    public Vector3 targetPosition;

    private void OnEnable()
    {
        //targetPosition = transform.position;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            // targetPosition = new Vector3(hit.point.x, rigidbody.position.y, rigidbody.position.z);
            targetPosition = hit.point;
        }
    }

    public float force;

    private void FixedUpdate()
    {
        var v = Magnitude(rigidbody.velocity);
        var targetDirection = Normalize(targetPosition - rigidbody.position);
        var distance = Distance(targetPosition, rigidbody.position);
        var a = acceleration;
        var p = distance; // plus t?

        var start = v * v / (2 * a) + p;
        var half = v * v / (4 * a) + 0.5f * p;
        var t = Sqrt(start / a);

        float targetV;
        if (p > half)
        {
            var time = 2 * t - Sqrt(2 * t * t - 2 * p / a) - Time.fixedDeltaTime;
            var newDistance = start - 0.5f * a * Sq(time - 2 * t);
            targetV = Sqrt(2f * a * (start - newDistance));
        }
        else
        {
            var time = Sqrt(2 * p / a) - Time.fixedDeltaTime;
            var newDistance = 0.5f * a * time * time;
            targetV = Sqrt(2f * a * newDistance);
        }

        var f = (targetDirection * targetV - rigidbody.velocity) / Time.fixedDeltaTime;
        var vel = ClampMagnitude(f, acceleration);
        force = vel.magnitude;
        rigidbody.AddForce(vel, ForceMode.Acceleration);
    }

    public static float Sq(float x) => x * x;
    public static float Rcp(float x) => 1f / x;
}