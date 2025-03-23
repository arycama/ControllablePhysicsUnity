using UnityEngine;

public class GeometryUtils
{
    public static Vector3 ClosestPointOnSphere(Vector3 point, Vector3 center, float radius)
    {
        var delta = point - center;

        // If the point is inside or on the sphere, return the point itself
        if (delta.magnitude <= radius)
            return point;

        // Normalize the direction vector and scale it to the radius
        return center + Vector3.Normalize(delta) * radius;
    }

    public static Vector2 ClosestPointOnCircle(Vector2 point, Vector2 center, float radius)
    {
        var delta = point - center;

        // If the point is inside or on the sphere, return the point itself
        if (delta.magnitude <= radius)
            return point;

        // Normalize the direction vector and scale it to the radius
        return center + delta.normalized * radius;
    }
}