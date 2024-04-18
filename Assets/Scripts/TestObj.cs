using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : MonoBehaviour
{
    public ObjectiveManager objManager;
    public float duration;

    // Floating animation
    private Vector3 startPos;
    public float a = 0.5f;
    public float f = 1f;
    public float r = 50f;
    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.time * Mathf.PI * f) * a;
        transform.position = tempPos;
        transform.Rotate(Vector3.up, r * Time.deltaTime);
    }



    // Actual Objective logic stuff
    private void OnTriggerEnter(Collider other)
    {
        if (other == null || other.gameObject == null) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Collided with Player");
            objManager.StartObjective("Test");
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        objManager.CompleteObjective("Test");
    }
}
