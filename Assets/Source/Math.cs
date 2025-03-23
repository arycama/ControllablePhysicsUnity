using System;

public static class Math
{
    public const float Pi = MathF.PI;
    public const float TwoPi = Pi * 2;
    public const float FourPi = Pi * 4;
    public const float HalfPi = Pi * 0.5f;
    public const float QuarterPi = Pi * 0.25f;

    public static float Radians(float x) => MathF.PI / 180 * x;
    public static float Degrees(float x) => 180 / MathF.PI * x;

    // Simple math utils
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
    public static float Rcp(float x) => 1.0f / x;
    public static float Sqrt(float x) => MathF.Sqrt(x);
    public static float Exp(float x) => MathF.Exp(x);
    public static float Pow(float x, float y) => MathF.Pow(x, y);

    // Utility
    public static float Lerp(float a, float b, float t) => a + t * (b - a);
    public static float InvLerp(float t, float a, float b) => (t - a) * Rcp(b - a);
    public static float Remap(float x, float prevMin, float prevMax, float newMin, float newMax) => Lerp(newMin, newMax, InvLerp(x, prevMin, prevMax));
    
    //public static float Rotate(Float2 p, float theta)
    //{
    //    var matrix = Float2x2.Rotate(theta);

    //}
}
