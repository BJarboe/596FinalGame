using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMarker : MonoBehaviour
{
    public ObjectiveManager objManager;
    public string obj_id;
    private Objective obj;


    // Floating animation
    private Vector3 startPos;
    private float a = 0.5f;
    private float f = 1f;
    private float r = 50f;
    private void Start()
    {
        startPos = transform.position;
        obj = objManager.GetObjective(obj_id);
    }
    private void Update()
    {
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.time * Mathf.PI * f) * a;
        transform.position = tempPos;
        transform.Rotate(Vector3.up, r * Time.deltaTime);

        if (obj.status == Objective.Status.Active || obj.status == Objective.Status.Completed)
            gameObject.SetActive(false);

    }
}
