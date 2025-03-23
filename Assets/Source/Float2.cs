using System;
using UnityEngine;

[Serializable]
public struct Float2
{
    public float x, y;

    public Float2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator Float2(float a) => new(a, a);

    public static implicit operator Float2(Vector2 a) => new(a.x, a.y);

    public static implicit operator Vector2(Float2 a) => new(a.x, a.y);

    public static Float2 operator +(Float2 a, Float2 b) => new(a.x + b.x, a.y + b.y);
    public static Float2 operator -(Float2 a, Float2 b) => new(a.x - b.x, a.y - b.y);
    public static Float2 operator *(Float2 a, Float2 b) => new(a.x * b.x, a.y * b.y);
    public static Float2 operator /(Float2 a, Float2 b) => new(a.x / b.x, a.y / b.y);
}