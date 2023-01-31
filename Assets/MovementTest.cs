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

    public float distance, distanceSoFar, halfPoint, totalDistance, targetSpeed, force, currentT, futureT, futureDistance;

    private void FixedUpdate()
    {
        var v = Magnitude(rigidbody.velocity);
        var targetDirection = Normalize(targetPosition - rigidbody.position);
        distance = Distance(targetPosition, rigidbody.position) - v * Time.fixedDeltaTime; // Quick hack to simulate 1 frame ahead, otherwise we start at 0
        var a = acceleration;
        var x = distance;

        distanceSoFar = Sq(v) * Rcp(2 * a);
        totalDistance = distance + distanceSoFar;
        halfPoint = Sqrt(totalDistance / a);

        // We need to check ahead of time, so first, calculate our total travel time, and step forward.
        //currentT = Sqrt(2 * x / a);
        currentT = 2 * halfPoint - Sqrt(2 * halfPoint * halfPoint - 2 * x / a);

        futureT = currentT - Time.fixedDeltaTime;

        futureDistance = totalDistance - 0.5f * a * Sq(futureT - 2 *  halfPoint);

        if (futureDistance  >= halfPoint)
            targetSpeed = Sqrt(2 * a * (totalDistance - futureDistance));
        else
            targetSpeed = Sqrt(2 * a * futureDistance);

        //force = Clamp((targetSpeed - v) / Time.fixedDeltaTime, -a, a);
        // rigidbody.AddForce(force * targetDirection, ForceMode.Acceleration);

        force = (rigidbody.velocity.magnitude - targetSpeed) / Time.fixedDeltaTime;

        rigidbody.velocity = targetSpeed* targetDirection;
    }

    public static float Sq(float x) => x * x;
    public static float Rcp(float x) => 1f / x;
}