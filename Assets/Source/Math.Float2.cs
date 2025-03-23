public static partial class Math
{
    public static Float2 Float2(float x, float y) => new Float2(x, y);

    public static float Dot(Float2 a, Float2 b) => a.x * b.x + a.y * b.y;

    public static float SquareMagnitude(Float2 a) => Dot(a, a);

    public static float Magnitude(Float2 a) => Sqrt(SquareMagnitude(a));

    public static Float2 ClampMagnitude(Float2 a, float maxMagnitude)
    {
        var squareMagnitude = SquareMagnitude(a);
        if (squareMagnitude <= Square(maxMagnitude))
            return a;

        var rcpMagnitude = Rsqrt(squareMagnitude);
        return a * rcpMagnitude * maxMagnitude;
    }
}