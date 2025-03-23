using UnityEngine;
using static UnityEngine.Mathf;

public static class MathExtensions
{
    public static bool SolveQuadratic(float a, float b, float c, out float result0, out float result1)
    {
        var discriminant = b * b - 4f * a * c;
        var sqrtDet = Sqrt(discriminant);

        result0 = (-b - sqrtDet) / (2f * a);
        result1 = (-b + sqrtDet) / (2f * a);

        return discriminant >= 0f;
    }

    public static float Sq(float x) => x * x;

    // Lerps using a (mostly) framerate-independent lerp
    public static float SmoothLerp(float a, float b, float t, float deltaTime)
    {
        return LerpUnclamped(b, a, Exp(-t * deltaTime));
    }

    // Lerps using a (mostly) framerate-independent lerp
    public static float SmoothLerp(float a, float b, float t)
    {
        return SmoothLerp(a, b, t, Time.deltaTime);
    }

    public static float SmoothLerpAngle(float a, float b, float t, float deltaTime)
    {
        return LerpAngle(b, a, Exp(-t * deltaTime));
    }

    // Smoothly increments a value
    public static float SmoothIncrement(float currentValue, float newValue, float damping, float deltaTime)
    {
        return SmoothLerp(currentValue, currentValue + newValue, damping, deltaTime);
    }

    // Smoothly increments a value
    public static float SmoothIncrement(float currentValue, float newValue, float damping)
    {
        return SmoothIncrement(currentValue, newValue, damping, Time.deltaTime);
    }
}
