using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target; // Reference to the GameObject to follow
    public Vector3 offset = new Vector3(0f, 2f, -10f); // Offset from the target position

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Calculate the desired position for the camera
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move the camera towards the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 5f);
        }
    }
}
