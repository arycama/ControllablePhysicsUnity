using UnityEngine;

public static class Vector2Extensions
{
    public static Vector3 X0Z(this Vector2 v) => new(v.x, 0, v.y);
}