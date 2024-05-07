using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientNpc : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the inspector
    public Vector3 front = Vector3.forward; // Modifiable front direction

    void Update()
    {
        if (player != null)
        {

            Vector3 directionToPlayer = player.position - transform.position;


            Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);


            Quaternion frontRotation = Quaternion.Inverse(Quaternion.LookRotation(front));


            transform.rotation = rotationToPlayer * frontRotation;
        }
    }
}
