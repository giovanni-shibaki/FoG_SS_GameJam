using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ResourceType
{
    Iron,
    Gold,
    Water
}

public class PlanetMovement : MonoBehaviour
{
    public Rigidbody2D planet;
    public float attachDistance = 10f; // Distance at which the object attaches to the player
    public float rotationSpeed = 30f; // Speed of rotation around the player
    public float circleInterval = 3f; // Interval between circle creation (in seconds)
    public GameObject circlePrefab; // Prefab of the circle object
    public ResourceType resourceType;

    private GameObject player; // Reference to the player GameObject
    private bool isAttached = false;
    private Vector3 initialOffset;
    private float circleTimer = 0f;

    void Awake() {
        planet = GetComponent<Rigidbody2D>();
        GetComponent<CircleCollider2D>().radius = planet.mass;
        transform.localScale = Vector3.one * planet.mass;
    }

    void Start()
    {
        // Calculate initial offset from the player
    }

    void Update()
    {
        if (isAttached)
        {
            RotateAroundPlayer();
            ManageCircleTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Player"))
        {
            AttachToPlayer(collider.gameObject);
        }
    }

    void AttachToPlayer(GameObject player)
    {
        isAttached = true;
        this.player = player;
        transform.SetParent(player.transform);
        initialOffset = transform.position - player.transform.position; // Update initial offset
        rotationSpeed *= Random.Range(0, 2) == 0 ? 1 : -1;
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
