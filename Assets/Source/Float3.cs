using System;
using UnityEngine;

[Serializable]
public struct Float3
{
    public float x, y, z;

    public Float3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static implicit operator Float3(float a) => new(a, a, a);

    public static implicit operator Float3(Vector3 a) => new(a.x, a.y, a.z);

    public static implicit operator Vector3(Float3 a) => new(a.x, a.y, a.z);

    public static Float3 operator +(Float3 a) => new(a.x, a.y, a.z);
    public static Float3 operator -(Float3 a) => new(-a.x, -a.y, -a.z);

    public static Float3 operator +(Float3 a, Float3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Float3 operator -(Float3 a, Float3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    public static Float3 operator *(Float3 a, Float3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
    public static Float3 operator /(Float3 a, Float3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);

    public static Bool3 operator <(Float3 a, int b) => new Bool3(a.x < b, a.y < b, a.z < b);
    public static Bool3 operator >(Float3 a, int b) => new Bool3(a.x > b, a.y > b, a.z > b);

    public Float2 xx => new(x, x);
    public Float2 xy => new(x, y);
    public Float2 xz => new(x, z);
    public Float2 yx => new(y, x);
    public Float2 yy => new(y, y);
    public Float2 yz => new(y, z);
    public Float2 zx => new(z, x);
    public Float2 zy => new(z, y);
    public Float2 zz => new(z, z);

    public Float3 xxx => new(x, x, x);
    public Float3 xxy => new(x, x, y);
    public Float3 xxz => new(x, x, z);
    public Float3 xyx => new(x, y, x);
    public Float3 xyy => new(x, y, y);
    public Float3 xyz => new(x, y, z);
    public Float3 xzx => new(x, z, x);
    public Float3 xzy => new(x, z, y);
    public Float3 xzz => new(x, z, z);

    public Float3 yxx => new(y, x, x);
    public Float3 yxy => new(y, x, y);
    public Float3 yxz => new(y, x, z);
    public Float3 yyx => new(y, y, x);
    public Float3 yyy => new(y, y, y);
    public Float3 yyz => new(y, y, z);
    public Float3 yzx => new(y, z, x);
    public Float3 yzy => new(y, z, y);
    public Float3 yzz => new(y, z, z);

    public Float3 zxx => new(z, x, x);
    public Float3 zxy => new(z, x, y);
    public Float3 zxz => new(z, x, z);
    public Float3 zyx => new(z, y, x);
    public Float3 zyy => new(z, y, y);
    public Float3 zyz => new(z, y, z);
    public Float3 zzx => new(z, z, x);
    public Float3 zzy => new(z, z, y);
    public Float3 zzz => new(z, z, z);
}