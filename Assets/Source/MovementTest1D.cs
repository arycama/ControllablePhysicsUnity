using UnityEngine;
using static UnityEngine.Mathf;

public class MovementTest1D : MonoBehaviour
{
    public float acceleration = 1;

    [SerializeField] private Camera camera;
    public bool test;

    public float target;
    public float velocity;
    public float accel;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                target = hit.point.x;
            }
        }
    }


    private void FixedUpdate()
    {
        var force = PhysicsExtensions.AccelerateToTarget(velocity, transform.position.x, target, acceleration, Time.fixedDeltaTime,test);

        accel = force;

        velocity += force * Time.fixedDeltaTime;

        transform.Translate(velocity * Time.fixedDeltaTime, 0, 0);
    }
}
