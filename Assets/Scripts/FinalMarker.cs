using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMarker : MonoBehaviour
{

    // Floating animation
    private Vector3 startPos;
    private float a = 0.5f;
    private float f = 1f;
    private float r = 50f;
    private void Start()
    {
        gameObject.SetActive(false);
        startPos = transform.position;
    }
    private void Update()
    {
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.time * Mathf.PI * f) * a;
        transform.position = tempPos;
        transform.Rotate(Vector3.up, r * Time.deltaTime);
    }
}
