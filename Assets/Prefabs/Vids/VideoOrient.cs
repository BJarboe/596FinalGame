using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoOrient : MonoBehaviour
{
    public Transform player; // Public variable to assign the player object in the editor

    void Update()
    {
        if (player == null) return; // If no player is assigned, do nothing
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, direction);
        transform.rotation = targetRotation;
    }
}
