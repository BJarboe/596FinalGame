using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private PlayerMovement player;
    private ObjectiveManager om;
    public string ID;    // change in inspector for each spawn point
    private bool isSet = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        om = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (om.GetObjective(ID).status == Objective.Status.Completed && !isSet)
        {
            isSet = true;
            player.SetRespawnPoint(transform.position);
        }
    }
}
