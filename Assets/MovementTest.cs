using UnityEngine;
using static UnityEngine.Mathf;
using static UnityEngine.Vector3;

public class MovementTest : MonoBehaviour
{
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private bool test = false;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Camera camera;

    private Vector3 targetPosition;

    private void OnEnable()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        //if (!Input.GetMouseButtonDown(0))
       //     return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            //targetPosition = new Vector3(hit.point.x, rigidbody.position.y, rigidbody.position.z);
            targetPosition = hit.point;
        }
    }

    public float force;

    private void FixedUpdate()
    {
        var targetDirection = Normalize(targetPosition - rigidbody.position);
        var distance = Distance(targetPosition, rigidbody.position);

        var a = acceleration;
        var dt = Time.fixedDeltaTime;
        var x = distance;

        // This accounts for the error introduced by dt
        var currentSpeed = 0.5f * a * (Sqrt((a * dt * dt + 8f * x) / a) - dt);

        // Time to reach target at current distance+ideal velocity
        var currentTime = (Sqrt(a * a * dt * dt + 8 * a * x) - a * dt) / (2 * a);

        // One frame forward
        var nextTime = currentTime - dt;

        // This is the distance goal for the next frame/timestep
        var nextDistance = 0.5f * a * nextTime * (nextTime + dt);

        // Now calcualte the speed required to get us to the target distance
        var targetDisplacement = Abs(nextDistance - distance);
        var targetSpeed = 2 * targetDisplacement / dt - currentSpeed;


        var velocityDelta = (targetSpeed * targetDirection - rigidbody.velocity) / dt;
        var force = ClampMagnitude(velocityDelta, a);
        this.force = force.magnitude;
        rigidbody.AddForce(force, ForceMode.Acceleration);
    }
}