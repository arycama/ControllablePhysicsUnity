using UnityEngine;
using static Math;

public static class PhysicsExtensions
{
    public static float VelocityAtTime(float velocity, float acceleration, float time)
    {
        return velocity + acceleration * time;
    }

    public static float DisplacementAtTime(float displacement, float velocity, float acceleration, float time)
    {
        return displacement + velocity * time + 0.5f * acceleration * time * time;
    }

    public static float VelocityFromAccelerationAndDisplacement(float acceleration, float displacement, float deltaTime)
    {
        return 0.5f * (Sqrt(acceleration * (acceleration * Sq(deltaTime) + 8 * displacement)) - acceleration * deltaTime);
    }

    public static float VelocityFromAccelerationAndDisplacement(float acceleration, float displacement)
    {
        return VelocityFromAccelerationAndDisplacement(acceleration, displacement, Time.deltaTime);
    }

    public static float DisplacementFromAccelerationAndVelocity(float acceleration, float velocity, float deltaTime)
    {
        return velocity * (acceleration * deltaTime + velocity) / (2 * acceleration);
    }

    public static float DisplacementFromAccelerationAndVelocity(float acceleration, float velocity)
    {
        return DisplacementFromAccelerationAndVelocity(acceleration, velocity, Time.deltaTime);
    }

    // Calculates an acceleration force to reach a target without overshooting
    public static float AccelerateToTarget(float velocity, float position, float target, float acceleration, float deltaTime)
    {
        var delta = target - position;
        var slowdownDistance = (2 * acceleration * delta - Square(velocity)) / (4 * acceleration);

        var slowdownTime0 = (-velocity + Sqrt(Square(velocity) + 2 * acceleration * slowdownDistance)) / acceleration;
        var slowdownTime1 = (velocity + Sqrt(0.5f * Square(velocity) - acceleration * delta)) / acceleration;

        var v0 = +acceleration * slowdownTime0 - acceleration * Abs(deltaTime - slowdownTime0);
        var v1 = -acceleration * slowdownTime1 + acceleration * Abs(deltaTime - slowdownTime1);

        var v2 = slowdownTime1 < 0 ? v0 : v1;
        var v3 = slowdownTime0 < 0 ? v1 : v0;
        var v4 = delta < 0 ? v2 : v3;

        return v4 / deltaTime;
    }

    public static float AccelerateToTarget(float velocity, float position, float target, float acceleration)
    {
        return AccelerateToTarget(velocity, position, target, acceleration, Time.deltaTime);
    }

    // Calculates an acceleration force to reach a target without overshooting
    public static Vector2 AccelerateToTarget(Vector2 velocity, Vector2 position, Vector2 target, float acceleration, float deltaTime)
    {
        var distance = Vector2.Distance(position, target);

        var speed = velocity.magnitude;
        SolveQuadratic(acceleration, speed, (0.5f * speed * speed - distance * acceleration) / (4 * acceleration), out _, out var timeToHalfPoint);

        float targetSpeed;
        if (deltaTime < timeToHalfPoint)
            targetSpeed = VelocityAtTime(speed, acceleration, deltaTime);
        else
            targetSpeed = VelocityAtTime(VelocityAtTime(speed, acceleration, timeToHalfPoint), -acceleration, deltaTime - timeToHalfPoint);

        var direction = (target - position).normalized;

        // Calculate acceleration required to get us to the desired velocity in a single frame
        var targetVelocity = direction * targetSpeed;
        var force = (targetVelocity - velocity) / deltaTime;

        return Vector2.ClampMagnitude(force, acceleration);
    }

    public static Vector2 AccelerateToTarget(Vector2 velocity, Vector2 position, Vector2 target, float acceleration)
    {
        return AccelerateToTarget(velocity, position, target, acceleration, Time.deltaTime);
    }

    // Calculates an acceleration force to reach a target without overshooting
    public static Vector3 AccelerateToTarget(Vector3 velocity, Vector3 position, Vector3 target, float acceleration, float deltaTime)
    {
#if false
        var d = Vector3.Distance(position, target);

        var v = velocity.magnitude;

        var a = acceleration;
        var b = v;
        var c = (-Sq(v) + 2 * v * Sqrt(0.5f * Sq(v) + d * a) - 2 * d * a) / (2 * a);
        var t = deltaTime;

        var inner = a * (d - a * d * t) + 0.5f * Sq(v);

        //var c = v * Sqrt(inner) / a + a * d * t - Sq(v) / (2 * a) - d;

        // Correct version for discreet physics
        //c = acceleration * distance * deltaTime - distance + (2 * speed) * (2 * speed) / (8 * acceleration);

        SolveQuadratic(a, b, c, out _, out var timeToHalfPoint);
        var targetSpeed = v + acceleration * (timeToHalfPoint - Abs(deltaTime - timeToHalfPoint));

        var direction = Vector3.Normalize(target - position);

        // Calculate acceleration required to get us to the desired velocity in a single frame
        var targetVelocity = direction * targetSpeed;
        var force = (targetVelocity - velocity) / deltaTime;

        // Clamp to max accel
        return Vector3.ClampMagnitude(force, acceleration);
#else
        var delta = target - position;

        var speed = Vector3.Dot(delta.normalized, velocity);
        var distance = Vector3.Dot(velocity.normalized, delta);

        var slowdownDistance = (2 * acceleration * distance - Square(speed)) / (4 * acceleration);

        var slowdownTime0 = (-speed + Sqrt(Square(speed) + 2 * acceleration * slowdownDistance)) / acceleration;
        var slowdownTime1 = (speed + Sqrt(0.5f * Square(speed) - acceleration * distance)) / acceleration;

        var v0 = +acceleration * slowdownTime0 - acceleration * Abs(deltaTime - slowdownTime0);
        var v1 = -acceleration * slowdownTime1 + acceleration * Abs(deltaTime - slowdownTime1);

        var v2 = slowdownTime1 < 0 ? v0 : v1;
        var v3 = slowdownTime0 < 0 ? v1 : v0;
        var v4 = distance < 0 ? v2 : v3;

        return delta.normalized * v4 / deltaTime;



        //var distance = Vector3.Distance(position, target);
        //var speed = velocity.magnitude;
        //var slowdownDistance = (2 * acceleration * distance - speed * speed) / (4 * acceleration);

        //SolveQuadratic(0.5f * acceleration, speed, -slowdownDistance, out _, out var slowdownTime);

        //float targetSpeed;
        //if (slowdownTime < 0)
        //    targetSpeed = speed - acceleration * deltaTime;
        //else
        //    targetSpeed = speed + acceleration * (slowdownTime - Abs(deltaTime - slowdownTime));

        //var direction = (target - position).normalized;
        //var targetVelocity = direction * targetSpeed;
        //var force = (targetVelocity - velocity) / deltaTime;

        //return Vector3.ClampMagnitude(force, acceleration);
#endif
    }

    public static Vector3 AccelerateToTarget(Vector3 velocity, Vector3 position, Vector3 target, float acceleration)
    {
        return AccelerateToTarget(velocity, position, target, acceleration, Time.deltaTime);
    }
}
