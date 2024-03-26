using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Iron,
    Gold,
    Water
}

public class PlanetMovement : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public float attachDistance = 10f; // Distance at which the object attaches to the player
    public float rotationSpeed = 30f; // Speed of rotation around the player
    public float circleInterval = 3f; // Interval between circle creation (in seconds)
    public GameObject circlePrefab; // Prefab of the circle object
    public ResourceType resourceType;

    private bool isAttached = false;
    private Vector3 initialOffset;
    private float circleTimer = 0f;

    void Start()
    {
        // Calculate initial offset from the player
        initialOffset = transform.position - player.transform.position;
    }

    void Update()
    {
        if (!isAttached && player != null)
        {
            // Calculate the distance between this object and the player
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // Check if the distance is less than the attach distance
            if (distance <= attachDistance)
            {
                AttachToPlayer();
            }
        }
        else if (isAttached)
        {
            RotateAroundPlayer();
            ManageCircleTimer();
        }
    }

    void AttachToPlayer()
    {
        isAttached = true;
        transform.SetParent(player.transform);
        initialOffset = transform.position - player.transform.position; // Update initial offset
    }

    void RotateAroundPlayer()
    {
        transform.RotateAround(player.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void ManageCircleTimer()
    {
        circleTimer += Time.deltaTime;
        if (circleTimer >= circleInterval)
        {
            CreateCircle();
            circleTimer = 0f;
        }
    }

    void CreateCircle()
    {
        // Instantiate the circle prefab at the object's position
        GameObject circle = Instantiate(circlePrefab, transform.position, Quaternion.identity);

        // Set the circle's destination to the player's position
        circle.GetComponent<CircleMovement>().SetPlayer(player);

        if(resourceType == ResourceType.Iron)
        {
            circle.tag = "Iron";
        } else if (resourceType == ResourceType.Gold) {
            circle.tag = "Gold";
        } else
        {
            circle.tag = "Water";
        }
    }
}
