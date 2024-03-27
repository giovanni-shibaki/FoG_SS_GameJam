using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private float escapeVelocity;
    private CircleCollider2D gravityCollider;
    
    void Awake() 
    {
        planet = GetComponent<Rigidbody2D>();
        gravityCollider = gameObject.AddComponent<CircleCollider2D>();
        gravityCollider.isTrigger = true;
    }

    private void OnEnable() 
    {
        planet.mass = Random.Range(1f, 4f);
        transform.localScale = Vector3.one * planet.mass * 0.8f;
        escapeVelocity = planet.mass * 0.5f;
    }

    void Start()
    {
        // Calculate initial offset from the player
        resourceType = (ResourceType)Random.Range(0, 3);
        gravityCollider.radius = planet.mass;
        player = GameObject.FindGameObjectWithTag("Player");
        initialOffset = transform.position - player.transform.position;
        circleInterval = Random.Range(1f, 4f);
    }

    void Update()
    {
        if (isAttached)
        {
            RotateAroundPlayer();
            ManageCircleTimer();
        }
    }

    void OnTriggerStay2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (isAttached) return;
        
        if (planet.velocity.magnitude < escapeVelocity)
        {
            AttachToPlayer(collision.gameObject);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 
                planet.mass * 1f * Time.deltaTime);
            planet.velocity /= 1.005f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) Destroy(gameObject);
    }

    public void AttachToPlayer(GameObject player)
    {
        if (player.GetComponent<PlayerResources>().numberOfPlanets <= 0) return;
        isAttached = true;
        this.player = player;
        transform.SetParent(player.transform);
        planet.velocity = new Vector2(0f, 0f);
        transform.position += transform.position.normalized * planet.mass * 0.1f * Random.Range(-1f, 1f);
        rotationSpeed *= (Random.Range(0, 2) == 0 ? 1 : -1) * Random.Range(0.1f, planet.mass);
        GetComponent<CircleCollider2D>().isTrigger = true;
        Destroy(gravityCollider);
        player.GetComponent<PlayerResources>().numberOfPlanets--;
    }

    private void RotateAroundPlayer()
    {
        transform.RotateAround(player.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void ManageCircleTimer()
    {
        circleTimer += Time.deltaTime;
        if (circleTimer >= circleInterval)
        {
            CreateCircle();
            circleTimer = 0f;
        }
    }

    private void CreateCircle()
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
