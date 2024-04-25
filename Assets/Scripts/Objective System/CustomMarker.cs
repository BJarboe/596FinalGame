using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMarker : MonoBehaviour
{
    public ObjectiveManager objManager;
    public string obj_id;
    private Objective obj;

    private Renderer rend;

    public Laundro2 l2;


    // Floating animation
    private Vector3 startPos;
    private float a = 0.5f;
    private float f = 1f;
    private float r = 50f;
    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        startPos = transform.position;
        obj = objManager.GetObjective(obj_id);
    }
    private void Update()
    {
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.time * Mathf.PI * f) * a;
        transform.position = tempPos;
        transform.Rotate(Vector3.up, r * Time.deltaTime);


        if (l2.startedCycle)
            rend.enabled = false;
        else if (l2.activateMarker)
            rend.enabled = true;
        

        if (obj.status == Objective.Status.Completed)
            gameObject.SetActive(false);
    }
}
