using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class SpawnerManager : MonoBehaviour
{
    public GameObject objectPrefab; // Prefab of the object to spawn
    public float spawnRate = 1f; // Rate of spawning objects (objects per second)
    public float spawnWidth = 10f; // Width of the spawn area
    public float spawnHeight = 10f; // Height of the spawn area
    public float minSpeed = 1f; // Minimum speed of spawned objects
    public float maxSpeed = 5f; // Maximum speed of spawned objects

    private float spawnTimer = 0f;

    void Update()
    {
        // Update the spawn timer
        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn a new object
        if (spawnTimer >= 1f / spawnRate)
        {
            SpawnObject();
            spawnTimer = 0f;
        }
    }

    void SpawnObject()
    {
        // Calculate random position within the spawn area
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth / 2f, spawnWidth / 2f), Random.Range(-spawnHeight / 2f, spawnHeight / 2f), -1f);

        // Instantiate the object at the random position
        GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // Randomize velocity of the spawned object
        Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomVelocity = new Vector2(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed));
            rb.velocity = randomVelocity.normalized * Random.Range(minSpeed, maxSpeed);
        }
        else
        {
            Debug.LogWarning("Object prefab does not have a Rigidbody2D component.");
        }
    }
}
