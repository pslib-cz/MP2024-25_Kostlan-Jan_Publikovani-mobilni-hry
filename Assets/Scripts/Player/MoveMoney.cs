using UnityEngine;

/// <summary>
/// Auto move money.
/// </summary>
public class MoveMoney : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        var transform1 = transform;
        Vector3 newPosition = transform1.position;
        newPosition.x += speed * Time.deltaTime;
        transform1.position = newPosition;
    }
}