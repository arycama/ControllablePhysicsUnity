using UnityEngine;

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

    private void FixedUpdate()
    {
        var a = acceleration;
        var v = rigidbody.velocity.magnitude;
        var p = Vector3.Distance(targetPosition, rigidbody.position);

        var p0 = v * v / (2 * a) + p;
        var h = v * v / (2 * a) + p / 2;

        var t = Mathf.Sqrt(p0 / a);
        var t0 = Mathf.Sqrt(2 * p / a);

        // Max required because when velocity is zero, floating point errors may result in this being slightly less than 0, causing a NaN
        var t1 = 2 * t - Mathf.Sqrt(Mathf.Max(0f, 2 * Sq(t) - 2 * p / a));

        var time = p <= h ? t0 : t1;
        time -= Time.fixedDeltaTime;

        var y0 = 0.5f * a * Sq(time);
        var y1 = p0 - 0.5f * a * Sq(time - 2f * t);
        var y = p < t ? y0 : y1;

        var targetDisplacement = Mathf.Abs(p - y);

        var targetDirection = Vector3.Normalize(targetPosition - rigidbody.position);
        var force = 2f * (targetDisplacement - v * Time.fixedDeltaTime) / Sq(Time.fixedDeltaTime);

        var clampedForce = Vector3.ClampMagnitude(targetDirection * force, acceleration);
        //var clampedForce = targetDirection * force;


        rigidbody.AddForce(clampedForce, ForceMode.Acceleration);

        //var targetVelocity = targetDisplacement ;
        //var velocityDelta = Vector3.ClampMagnitude((targetDirection * targetVelocity - rigidbody.velocity) / Time.fixedDeltaTime, acceleration);
        //rigidbody.AddForce(velocityDelta, ForceMode.Acceleration);

    }

    public static float Sq(float x) => x * x;
}