using UnityEngine;

public static class Vector3Extensions
{
    public static Vector2 XZ(this Vector3 v) => new(v.x, v.z);
    public static Vector3 Y0(this Vector3 v) => new(v.x, 0, v.z);
}
