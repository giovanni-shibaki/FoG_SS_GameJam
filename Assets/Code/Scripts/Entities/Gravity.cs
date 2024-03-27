using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    
    
    private void Start() 
    {
    }

    // private void OnTriggerEnter2D(Collider2D collider) 
    // {
    //     if (collider.gameObject.CompareTag("Player"))
    //     {
    //         planet.AddForce(((Vector2)collider.transform.position - planet.position) * planet.mass);
    //     }
    // }

    // private void OnTriggerStay2D(Collision2D collider) {
    //     // if (collider.gameObject.CompareTag("Player") && planet.velocity.magnitude < escapeVelocity)
    //     if (collider.gameObject.CompareTag("Player") )
    //     {
    //         AttachToPlayer(collider.gameObject);
    //     }
    // }
    
    
    private void AttachToPlayer(GameObject player)
    {
        GetComponentInParent<PlanetMovement>().AttachToPlayer(player);
    }
}
