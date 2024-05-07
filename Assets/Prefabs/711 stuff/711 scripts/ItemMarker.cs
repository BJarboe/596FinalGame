using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMarker : MonoBehaviour
{
    [SerializeField]
    private GameObject item;

    // Floating animation
    private Vector3 startPos;
    [SerializeField]
    private float amplitude = 0.5f;
    [SerializeField]
    private float frequency = 1f;
    [SerializeField]
    private float rotation = 50f;
    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
        transform.Rotate(Vector3.up, rotation * Time.deltaTime);

        if (!item.activeSelf)
            gameObject.SetActive(false);
    }
}
