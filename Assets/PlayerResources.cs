using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class PlayerResources : MonoBehaviour
{
    private int ironCount = 0;
    private int goldCount = 0;
    private int waterCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check the tag of the collided object to determine the type of resource
        switch (collision.gameObject.tag)
        {
            case "Iron":
                ironCount++;
                Debug.Log("Iron collected. Total iron count: " + ironCount);
                break;
            case "Gold":
                goldCount++;
                Debug.Log("Gold collected. Total gold count: " + goldCount);
                break;
            case "Water":
                waterCount++;
                Debug.Log("Water collected. Total water count: " + waterCount);
                break;
            default:
                Debug.LogWarning("Unknown resource type collided.");
                break;
        }

        // Destroy the collided object
        Destroy(collision.gameObject);
    }

    public int getIronCount() { return ironCount; }

    public int getGoldCount() {  return goldCount; }

    public int getWaterCount() {  return waterCount; }
}
