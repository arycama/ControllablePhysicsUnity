using UnityEngine;

public partial struct Float3
{
    public float x, y, z;

    public Float3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

public partial struct Float3
{
    public static implicit operator Vector3(Float3 v) => new(v.x, v.y, v.z);

}