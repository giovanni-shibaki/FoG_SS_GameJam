using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    public Transform player; // Reference to the player GameObject
    public float attachDistance = 10f; // Distance at which the object attaches to the player
    public float rotationSpeed = 30f; // Speed of rotation around the player

    private bool isAttached = false;
    private Vector3 initialOffset;

    void Start()
    {
        // Calculate initial offset from the player
        initialOffset = transform.position - player.position;
    }

    void Update()
    {
        if (!isAttached && player != null)
        {
            // Calculate the distance between this object and the player
            float distance = Vector3.Distance(transform.position, player.position);

            // Check if the distance is less than the attach distance
            if (distance <= attachDistance)
            {
                AttachToPlayer();
            }
        }
        else if (isAttached)
        {
            RotateAroundPlayer();
        }
    }

    void AttachToPlayer()
    {
        isAttached = true;
        initialOffset = transform.position - player.position; // Update initial offset
    }

    void RotateAroundPlayer()
    {
        // Update position relative to the player's current position
        transform.position = player.position + initialOffset;

        // Rotate around the player's current position on the z-axis
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
