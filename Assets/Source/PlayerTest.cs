using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera camera;

    public static Vector2 Rotate(Vector2 v, float theta)
    {
        return new Vector2(
            v.x * Mathf.Cos(theta) - v.y * Mathf.Sin(theta),
            v.x * Mathf.Sin(theta) + v.y * Mathf.Cos(theta)
        );
    }

    public void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var localMovement = Vector2.ClampMagnitude(new(horizontal, vertical), 1.0f);

        var worldMovement = Rotate(localMovement, -camera.transform.rotation.eulerAngles.y * Mathf.Deg2Rad);

        var movement = worldMovement * moveSpeed * Time.deltaTime;
        transform.position += movement.X0Z();
    }
}
