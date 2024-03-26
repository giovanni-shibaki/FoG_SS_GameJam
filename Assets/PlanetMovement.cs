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
    public float rotationSpeed = 30f; // Speed of rotation around the player
    public float circleInterval = 3f; // Interval between circle creation (in seconds)
    public GameObject circlePrefab; // Prefab of the circle object

    private Rigidbody2D planet;
    private ResourceType resourceType;
    private GameObject player; // Reference to the player GameObject
    private bool isAttached = false;
    private Vector3 initialOffset;
    private float circleTimer = 0f;
    private CircleCollider2D collider2;

    void Awake() {
        planet = GetComponent<Rigidbody2D>();
        planet.mass = Random.Range(1f, 4f);
        GetComponent<CircleCollider2D>().radius = planet.mass;
        transform.localScale = Vector3.one * planet.mass;
        collider2 = gameObject.AddComponent<CircleCollider2D>();
    }

    void Start()
    {
        // Calculate initial offset from the player
        resourceType = (ResourceType)Random.Range(0, 3);
        player = GameObject.FindGameObjectWithTag("Player");
        initialOffset = transform.position - player.transform.position;
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
        planet.velocity = new Vector2(0f, 0f);
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
            circle.GetComponent<SpriteRenderer>().material.color = new Color(0.5849056f, 0.5849056f, 0.5849056f, 1f);
        } else if (resourceType == ResourceType.Gold) {
            circle.tag = "Gold";
            circle.GetComponent<SpriteRenderer>().material.color = new Color(1f, 0.8745098f, 0f, 1f);
        } else
        {
            circle.tag = "Water";
            circle.GetComponent<SpriteRenderer>().material.color = new Color(0.1098039f, 0.6392157f, 0.9254902f, 1f);
        }
    }
}
