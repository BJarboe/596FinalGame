using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnManager : MonoBehaviour
{
    private EnemyBehavior enemy;
    private ObjectiveManager om;
    public string ID;    // change in inspector for each spawn point
    private bool isSet = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehavior>();
        om = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (om.GetObjective(ID).status == Objective.Status.Completed && !isSet)
        {
            isSet = true;
            enemy.SetRespawnPoint(transform.position);
        }
    }
}
