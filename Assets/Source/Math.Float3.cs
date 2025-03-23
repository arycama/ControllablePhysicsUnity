public static partial class Math
{
    public static Float3 Float3(float x, float y, float z) => new Float3(x, y, z);

    // Utility
    public static Float3 Select(Bool3 c, Float3 a, Float3 b) => Float3(c.x ? a.x : b.x, c.y ? a.y : b.y, c.z ? a.z : b.z);

    // Common math
    public static Float3 Abs(Float3 a) => Float3(Abs(a.x), Abs(a.y), Abs(a.z));
    public static Float3 Square(Float3 a) => a * a;
    public static Float3 Sqrt(Float3 a) => Float3(Sqrt(a.x), Sqrt(a.y), Sqrt(a.z));

    // Vector
    public static float Dot(Float3 a, Float3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

    public static float SquareMagnitude(Float3 a) => Dot(a, a);
    public static float Magnitude(Float3 a) => Sqrt(SquareMagnitude(a));
    public static float Distance(Float3 a, Float3 b) => Magnitude(b - a);

    public static Float3 Normalize(Float3 a) => a * Rsqrt(SquareMagnitude(a));

    public static Float3 ClampMagnitude(Float3 a, float maxMagnitude)
    {
        var squareMagnitude = SquareMagnitude(a);
        if (squareMagnitude <= Square(maxMagnitude))
            return a;

        var rcpMagnitude = Rsqrt(squareMagnitude);
        return a * rcpMagnitude * maxMagnitude;
    }

    public static float CosAngle(Float3 a, Float3 b)
    {
        var magnitude = Sqrt(SquareMagnitude(a) * SquareMagnitude(b));
        return Dot(a, b) / magnitude;
    }
}
