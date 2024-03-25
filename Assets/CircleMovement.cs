using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    private GameObject player; // Reference to the player GameObject
    private float speed = 2f;

    // Update is called once per frame
    void Update()
    {
        // Move towards the destination
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (player.GetComponent<PlayerMovement>().getSpeed() + speed) * Time.deltaTime);

        // If the circle reaches the destination, destroy it
        if (transform.position == player.transform.position)
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
