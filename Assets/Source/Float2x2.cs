using static Math;

public struct Float2x2
{
    public float m00, m10, m01, m11;

    public Float2x2(float m00, float m10, float m01, float m11)
    {
        this.m00 = m00;
        this.m10 = m10;
        this.m01 = m01;
        this.m11 = m11;
    }

    public static Float2x2 Rotate(float theta)
    {
        SinCos(theta, out var sinTheta, out var cosTheta);
        return new(cosTheta, -sinTheta, sinTheta, cosTheta);
    }
}