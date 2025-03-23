using System;

public static partial class Math
{
    public const float Pi = MathF.PI;
    public const float TwoPi = Pi * 2;
    public const float FourPi = Pi * 4;
    public const float HalfPi = Pi * 0.5f;
    public const float QuarterPi = Pi * 0.25f;

    public static float Radians(float x) => MathF.PI / 180 * x;
    public static float Degrees(float x) => 180 / MathF.PI * x;

    // Simple math utils
    public static float Select(bool c, float a, float b) => c ? a : b;
    public static float Sign(float x) => x >= 0 ? 1 : -1;
    public static float Abs(float x) => MathF.Abs(x);
    public static float Min(float x, float y) => x <= y ? x : y;
    public static float Max(float x, float y) => x >= y ? x : y;
    public static float Clamp(float x, float min, float max) => x < min ? min : (x > max ? max : x);

    // Trig/inv trig
    public static void SinCos(float theta, out float sinTheta, out float cosTheta)
    {
        sinTheta = Sin(theta);
        cosTheta = Cos(theta);
    }

    public static float Sin(float x) => MathF.Sin(x);
    public static float Cos(float x) => MathF.Cos(x);
    public static float Tan(float x) => MathF.Tan(x);

    public static float Asin(float x) => MathF.Asin(x);
    public static float Acos(float x) => MathF.Acos(x);
    public static float Atan(float x) => MathF.Atan(x);
    public static float Atan2(float y, float x) => MathF.Atan2(y, x);

    // Transcendental etc
    public static float Square(float x) => x * x;
    public static float Cube(float x) => x * x * x;

    public static float Rcp(float x) => 1.0f / x;
    public static float Sqrt(float x) => MathF.Sqrt(x);
    public static float Rsqrt(float x) => Rcp(MathF.Sqrt(x));
    public static float Exp(float x) => MathF.Exp(x);
    public static float Pow(float x, float y) => MathF.Pow(x, y);
    public static float Mod(float x, float y)
    {
        var remainder = x % y;
        return remainder >= 0 ? remainder : remainder + y;
    }

    // Utility
    public static float Lerp(float a, float b, float t) => a + t * (b - a);
    public static float InvLerp(float t, float a, float b) => (t - a) * Rcp(b - a);
    public static float Remap(float x, float prevMin, float prevMax, float newMin, float newMax) => Lerp(newMin, newMax, InvLerp(x, prevMin, prevMax));
    public static float Damp(float a, float b, float t, float dt) => Lerp(a, b, 1 - Exp(-t * dt));

    public static bool SolveQuadratic(float a, float b, float c, out float result0, out float result1)
    {
        var discriminant = b * b - 4f * a * c;
        var sqrtDet = Sqrt(discriminant);

        result0 = (-b - sqrtDet) / (2f * a);
        result1 = (-b + sqrtDet) / (2f * a);

        return discriminant >= 0f;
    }

    public static float Sq(float x) => x * x;

    public static float SmoothLerp(float a, float b, float t, float deltaTime)
    {
        return Lerp(b, a, Exp(-t * deltaTime));
    }

    public static float SmoothLerpAngle(float a, float b, float t, float deltaTime)
    {
        return UnityEngine.Mathf.LerpAngle(b, a, Exp(-t * deltaTime));
    }

    public static float SmoothIncrement(float currentValue, float newValue, float damping, float deltaTime)
    {
        return SmoothLerp(currentValue, currentValue + newValue, damping, deltaTime);
    }

    // Vector/matrix
    public static Float2 Mul(Float2x2 m, Float2 p)
    {
        return p.x * m.c0 + p.y * m.c1;
    }

    public static Float2 Rotate(Float2 p, float theta)
    {
        SinCos(theta, out var sinTheta, out var cosTheta);
        var matrix = new Float2x2(cosTheta, -sinTheta, sinTheta, cosTheta);
        return Mul(matrix, p);
    }
}
