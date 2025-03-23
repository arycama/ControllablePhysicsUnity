using UnityEngine;
using static UnityEngine.Mathf;

public static class SphericalCoordinates
{
    public static Vector3 SphericalToCartesian(float theta, float phi, float distance)
    {
        var sinTheta = Sin(theta);
        var cosTheta = Cos(theta);
        var sinPhi = Sin(phi);
        var cosPhi = Cos(phi);
        return new Vector3(sinPhi * cosTheta, cosPhi, sinPhi * sinTheta) * distance;

        //var up = new Vector3(cosPhi * sinTheta, sinPhi, -cosPhi * cosTheta);
        //var fwd = new Vector3(-sinPhi * sinTheta, cosPhi, sinPhi * cosTheta);
    }

    //public static Vector3 CartesianToSpherical(this Vector3 cartesian)
    //{

    //}
}