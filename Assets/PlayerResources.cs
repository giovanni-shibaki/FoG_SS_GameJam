using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class PlayerResources : MonoBehaviour
{
    public Transform planet; // Reference to the planet GameObject

    public float sizeIncreaseAmount = 0.5f; // Amount to increase the size of the planet
    public float cameraSizeIncrease = 1f; // Amount to increase the camera distance
    public int numberOfPlanets = 5;

    private int ironCount = 0;
    private int goldCount = 0;
    private int waterCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check the tag of the collided object to determine the type of resource
        switch (collision.gameObject.tag)
        {
            case "Iron":
                if(ironCount < 100)
                    ironCount += 2;
                Debug.Log("Iron collected. Total iron count: " + ironCount);
                break;
            case "Gold":
                if(goldCount < 100)
                    goldCount += 2;
                Debug.Log("Gold collected. Total gold count: " + goldCount);
                break;
            case "Water":
                if(waterCount < 100)
                    waterCount += 2;
                Debug.Log("Water collected. Total water count: " + waterCount);
                break;
            default:
                Debug.LogWarning("Unknown resource type collided.");
                break;
        }
    }

    public int getIronCount() { return ironCount; }

    public int getGoldCount() {  return goldCount; }

    public int getWaterCount() {  return waterCount; }

    void Update()
    {
        // Check if the player pressed the B, N, or M keys
        if (Input.GetKeyDown(KeyCode.B))
        {
            UseResource("Iron");
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            UseResource("Gold");
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            UseResource("Water");
        }
    }

    void UseResource(string resourceType)
    {
        switch (resourceType)
        {
            case "Iron":
                if (this.ironCount > 10)
                {
                    this.ironCount -= 10;
                    IncreasePlanetSize();
                }
                break;
            case "Gold":
                if (this.goldCount > 10)
                {
                    this.goldCount -= 10;
                    IncreaseCameraSize();
                }
                break;
            case "Water":
                if (this.waterCount > 10)
                {
                    this.waterCount -= 10;
                    IncreaseNumberOfPlanets();
                }
                break;
            default:
                Debug.LogWarning("Unknown resource type used.");
                break;
        }

        void IncreasePlanetSize()
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
        }

        void IncreaseCameraSize()
        {
            Camera.main.orthographicSize += cameraSizeIncrease;
        }

        void IncreaseNumberOfPlanets()
        {
            this.numberOfPlanets += 2;
        }
    }
}
