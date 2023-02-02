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

    public float p, p0, h, t, t0, t1, x, d0, d1, mid, y, targetDisplacement, dist, force;

    private void FixedUpdate()
    {
        var a = acceleration;
        var v = rigidbody.velocity.magnitude;
        p = Distance(targetPosition, rigidbody.position);
        var d = Time.fixedDeltaTime;

        p0 = v * (d * a + v) / (2 * a) + p;
        h = p0 / 2;

        t = Sqrt(p0 / a);

        t0 = Sqrt(Sq(a) * Sq(d) + 8 * a * p) / (2 * a) - d / 2;
        t1 = 2 * t - Sqrt(Max(0, 8 * Sq(a) * Sq(t) - Sq(a) * Sq(d) - 8 * a * p)) / (2 * a) - d / 2;

        x = p <= h ? t0 : t1;
        x -= Time.fixedDeltaTime;

        d0 = a * x * (x + d) / 2;
        d1 = -a * (2 * Sq(x) + 2 * d * x - 8 * x * t + Sq(d) + 4 * Sq(t) - 4 * d * t) / 4;
        mid = Sqrt(Sq(a) * Sq(d) + 8 * a * h) / (2 * a) - d / 2;
        y = p < mid ? d0 : d1;

        targetDisplacement = Abs(p - y);

        var targetDirection = Normalize(targetPosition - rigidbody.position);
        // force = (targetDisplacement - v * Time.fixedDeltaTime) / Sq(Time.fixedDeltaTime);

        //force = (targetDisplacement - 2f * v * d) / Sq(d);
        //force = (targetDisplacement - v - d * v) / (0.5f * (1 + d)) / d;

        dist = p0 - y;

        force = Abs(p0 - y) / (0.5f * x * (x * d));

        var clampedForce = ClampMagnitude(targetDirection * force, acceleration);
        rigidbody.AddForce(clampedForce, ForceMode.Acceleration);
    }

    public static float Sq(float x) => x * x;
}