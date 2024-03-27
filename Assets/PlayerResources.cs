using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class PlayerResources : MonoBehaviour
{
    public Transform planet; // Reference to the planet GameObject
    public Camera mainCamera; // Reference to the main camera

    public float sizeIncreaseAmount = 0.5f; // Amount to increase the size of the planet
    public float cameraSizeIncrease = 1f; // Amount to increase the camera distance
    public float speedIncreaseAmount = 1f;
    public int numberOfPlanets = 5;

    private int ironCount = 0;
    private int goldCount = 0;
    private int waterCount = 0;

    public int health = 100;

    private HealthManager healthBarManager;

    // Update is called once per frame
    void Update()
    {
        // Check if the player pressed the B, N, or M keys
        if (Input.GetKeyDown(KeyCode.B))
        {
            IncreasePlanetSize();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            IncreasePlanetSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            IncreaseNumberOfPlanets();
        }
    }

    void Start()
    {
        healthBarManager = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check the tag of the collided object to determine the type of resource
        switch (collision.gameObject.tag)
        {
            case "Iron":
                if (ironCount < 100)
                    ironCount += 2;
                Debug.Log("Iron collected. Total iron count: " + ironCount);
                break;
            case "Gold":
                if (goldCount < 100)
                    goldCount += 2;
                Debug.Log("Gold collected. Total gold count: " + goldCount);
                break;
            case "Water":
                if (waterCount < 100)
                    waterCount += 2;
                Debug.Log("Water collected. Total water count: " + waterCount);
                break;
            case "Planet":
                break;
            default:
                Debug.LogWarning("Unknown resource type collided.");
                Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
                TakeDamage((int)rb.mass*10);
                break;
        }
    }

    public int getIronCount() { return ironCount; }

    public int getGoldCount() { return goldCount; }

    public int getWaterCount() { return waterCount; }

    void IncreasePlanetSize()
    {
        if(ConsumeResources(0.4f, 0.3f, 0.3f))
        {
            // Calculate the scaling factor
            Vector3 scaleChange = new Vector3(sizeIncreaseAmount, sizeIncreaseAmount, 0f);
            Vector3 scaleFactor = new Vector3(1f - scaleChange.x / planet.localScale.x, 1f - scaleChange.y / planet.localScale.y, 1f);

            // Apply the scaling factor to the planet and its children
            planet.localScale += scaleChange;
            foreach (Transform child in planet)
            {
                child.localScale = Vector3.Scale(child.localScale, scaleFactor);
            }

            // Increase camera size
            mainCamera.orthographicSize += cameraSizeIncrease;
        }
    }

    void IncreasePlanetSpeed()
    {
        // Consume resources proportionally
        if(ConsumeResources(0.3f, 0.3f, 0.4f))
        {
            // Find the object with the PlayerMovement component
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    // Get the current speed and increase it
                    float currentSpeed = playerMovement.getSpeed();
                    float newSpeed = currentSpeed + speedIncreaseAmount;
                    playerMovement.setSpeed(newSpeed); // Set the new speed
                }
                else
                {
                    Debug.LogWarning("PlayerMovement component not found on player object.");
                }
            }
            else
            {
                Debug.LogWarning("Player object not found.");
            }
        }
    }

    void IncreaseNumberOfPlanets()
    {
        // Consume resources proportionally
        if (ConsumeResources(0.3f, 0.3f, 0.4f))
        {
            this.numberOfPlanets += 2;
            Debug.Log("Number of planets increased. Total number of planets: " + numberOfPlanets);
        }
    }

    bool ConsumeResources(float ironProportion, float goldProportion, float waterProportion)
    {
        int totalResources = ironCount + goldCount + waterCount;

        // Calculate the amount to consume from each resource
        int ironToConsume = Mathf.RoundToInt(ironCount * ironProportion);
        int goldToConsume = Mathf.RoundToInt(goldCount * goldProportion);
        int waterToConsume = Mathf.RoundToInt(waterCount * waterProportion);

        // Consume resources
        if(ironCount > ironToConsume && goldCount > goldToConsume && waterCount > waterToConsume)
        {
            ironCount -= ironToConsume;
            goldCount -= goldToConsume;
            waterCount -= waterToConsume;

            Debug.Log("Resources consumed - Iron: " + ironToConsume + ", Gold: " + goldToConsume + ", Water: " + waterToConsume);

            return true;
        }
        return false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBarManager.TakeDamage(damage);

        if(health <= 0)
        {
            Transform objTransform = GameObject.FindGameObjectWithTag("GameOver").transform;

            // Get the current position
            Vector3 newPosition = objTransform.position;
            newPosition.x = 960;

            // Assign the modified position back to the Transform
            objTransform.position = newPosition;
        }
    }
}
